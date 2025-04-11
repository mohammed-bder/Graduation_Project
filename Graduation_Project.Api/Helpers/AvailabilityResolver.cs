using AutoMapper;
using Graduation_Project.Api.DTO.Doctors;

namespace Graduation_Project.Api.Helpers
{
    public class AvailabilityResolver : IValueResolver<Doctor, SortingDoctorDto, string>
    {

        public string Resolve(Doctor source, SortingDoctorDto destination, string destMember, ResolutionContext context)
        {
            var availability = context.Items.ContainsKey("AvailabilityFilter")
            ? (AvailabilityFilter?)context.Items["AvailabilityFilter"]
            : null;

            var today = DateOnly.FromDateTime(TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Egypt Standard Time"));
            var tomorrow = DateOnly.FromDateTime(TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Egypt Standard Time").AddDays(1));
            // Calculate the start of the current week (Sunday, or adjust as needed)
            DateOnly endOfWeek = today.AddDays(6);
            // Find the next available schedule (either WorkSchedule or ScheduleException)
            var nextAvailableScheduleException = source.ScheduleExceptions
                .Where(se => se.IsAvailable && se.Date >= today)
                .OrderBy(se => se.Date)
                .FirstOrDefault();

            //var nextAvailableWorkSchedule = source.WorkSchedules
            //    .OrderBy(ws => ((int)ws.Day - (int)today.DayOfWeek + 7) % 7)
            //    .FirstOrDefault();

            var workScheduleExceptions = source.WorkSchedules
                .Select(ws =>
                {
                    // Get all the future dates for the given DayOfWeek of WorkSchedule
                    var scheduleDates = GetDatesForDayOfWeek(ws.Day, today, today.AddMonths(1));
                    return scheduleDates.Select(date => new ScheduleException
                    {
                        Date = date,
                        IsAvailable = true, // Mark as available by default
                        StartTime = ws.StartTime,
                        EndTime = ws.EndTime
                    }).ToList();
                })
                .SelectMany(se => se) // Flatten the list of lists of ScheduleExceptions
                .ToList();

            var validWorkScheduleExceptions = workScheduleExceptions
                .Where(se => !source.ScheduleExceptions
                    .Any(conflictingSe => conflictingSe.Date == se.Date && !conflictingSe.IsAvailable)) // Filter out days with unavailable ScheduleExceptions
                .ToList();

            var nextAvailableWorkSchedule = validWorkScheduleExceptions
                .OrderBy(se => se.Date) // Order by the date to get the next available day
                .FirstOrDefault();

            if (availability == AvailabilityFilter.AllTimes)
            {
                if (nextAvailableScheduleException != null)
                {
                    string dateDisplay = (nextAvailableScheduleException.Date >= today && nextAvailableScheduleException.Date <= endOfWeek)
                        ? nextAvailableScheduleException.Date.DayOfWeek.ToString()  // Show the day name (e.g., "Monday")
                        : nextAvailableScheduleException.Date.ToString("dd/MM/yyyy"); // Show the full date (e.g., "31/03/2025")

                    return $"Open Time: {nextAvailableScheduleException.StartTime:hh:mm tt} - {nextAvailableScheduleException.EndTime:hh:mm tt} ({dateDisplay})";
                }
                else if (nextAvailableWorkSchedule != null)
                {
                    string dateDisplay = (nextAvailableWorkSchedule.Date >= today && nextAvailableWorkSchedule.Date <= endOfWeek)
                        ? nextAvailableWorkSchedule.Date.DayOfWeek.ToString()  // Show the day name (e.g., "Monday")
                        : nextAvailableWorkSchedule.Date.ToString("dd/MM/yyyy"); // Show the full date (e.g., "31/03/2025")
                    return $"Open Time: {nextAvailableWorkSchedule.StartTime:hh:mm tt} - {nextAvailableWorkSchedule.EndTime:hh:mm tt} ({dateDisplay})";
                }
            }
            else if (availability == AvailabilityFilter.Today || availability == AvailabilityFilter.Tomorrow)
            {
                var targetDate = availability == AvailabilityFilter.Today ? today : tomorrow;
                var dayLabel = availability == AvailabilityFilter.Tomorrow ? $" ({targetDate.DayOfWeek})" : "";

                var scheduleException = source.ScheduleExceptions
                    .FirstOrDefault(se => se.Date == targetDate && se.IsAvailable);

                if (scheduleException != null)
                    return $"Open Time: {scheduleException.StartTime:hh:mm tt} - {scheduleException.EndTime:hh:mm tt}{dayLabel}";

                var workSchedule = source.WorkSchedules
                    .FirstOrDefault(ws => ws.Day == targetDate.DayOfWeek);

                if (workSchedule != null)
                    return $"Open Time: {workSchedule.StartTime:hh:mm tt} - {workSchedule.EndTime:hh:mm tt}{dayLabel}";
            }

            // If availability is NULL, return nearest available day OR "Unavailable"
            if (availability == null)
            {
                if (nextAvailableScheduleException != null)
                {
                    string dayLabel = GetFormattedDayLabel(nextAvailableScheduleException.Date, today);
                    return $"Open Time: {nextAvailableScheduleException.StartTime:hh:mm tt} - {nextAvailableScheduleException.EndTime:hh:mm tt} {dayLabel}";
                }
                else if (nextAvailableWorkSchedule != null)
                {
                    string dayLabel = GetFormattedDayLabel(nextAvailableWorkSchedule.Date, today);
                    return $"Open Time: {nextAvailableWorkSchedule.StartTime:hh:mm tt} - {nextAvailableWorkSchedule.EndTime:hh:mm tt} {dayLabel}";
                }
                return "Unavailable"; // No available schedule
            }

            return ""; // Default empty case
        }

        private string GetFormattedDayLabel(DateOnly date, DateOnly today)
        {
            DateOnly endOfWeek = today.AddDays(6);

            if (date == today)
                return "(Today)";
            else if (date == today.AddDays(1))
                return $"({date.DayOfWeek})"; // Show day name for Tomorrow
            else if (date < endOfWeek)
                return $"({date.DayOfWeek})"; // Show day name if it's in this week
            else
                return $"({date:dd/MM/yyyy})"; // Show full date if it's in the next week or later
        }

        private IEnumerable<DateOnly> GetDatesForDayOfWeek(DayOfWeek targetDay, DateOnly startDate, DateOnly endDate)
        {
            List<DateOnly> dates = new List<DateOnly>();
            DateOnly current = startDate;

            while (current <= endDate)
            {
                if (current.DayOfWeek == targetDay)
                {
                    dates.Add(current);
                }
                current = current.AddDays(1); // Move to the next day
            }

            return dates;
        }
    }
}
