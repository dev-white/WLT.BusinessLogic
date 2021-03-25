using System;

namespace WLT.BusinessLogic.BAL
{
    public class TimeRangeConverter
    {
        public string ConvertTimeRangeToUTC(string UserTimeRange, string TimeZone)
        {
            var UserTimeZone = TimeZoneInfo.FindSystemTimeZoneById(TimeZone); //E. Africa Standard Time

            var now = DateTimeOffset.UtcNow;
            TimeSpan UserOffset = UserTimeZone.GetUtcOffset(now);

            var hours = 0;
            var minutes = 0;

            hours = UserOffset.Hours;
            minutes = UserOffset.Minutes;

            var timeSpan = (hours * 2) + (minutes / 30);

            var timeRangeArr = UserTimeRange.Split(',');

            var timeRangeConvertedArr = new string[timeRangeArr.Length];

            for (int i = 0; i < timeRangeArr.Length; i++)
            {
                timeRangeConvertedArr[i] = "0";
            }


            if (timeSpan == 0) //Don't shift
            {
                timeRangeConvertedArr = timeRangeArr;
            }
            else if (timeSpan > 0) //Shift Left
            {
                //Array.Copy(timeRangeArr, timeSpan, timeRangeConvertedArr, 0, timeRangeArr.Length - timeSpan);

                timeRangeConvertedArr = Shift(timeRangeArr, -(timeSpan));
            }
            else //Shift Right
            {
                //Array.Copy(timeRangeArr, 0, timeRangeConvertedArr, timeSpan, timeRangeArr.Length - timeSpan);
                timeRangeConvertedArr = Shift(timeRangeArr, timeSpan);
            }

            return string.Join(",", timeRangeConvertedArr);
        }

        public string ConvertTimeRangeToUserTime(string UTCTimeRange, string TimeZone)
        {
            var UserTimeZone = TimeZoneInfo.FindSystemTimeZoneById(TimeZone); //E. Africa Standard Time

            var now = DateTimeOffset.UtcNow;
            TimeSpan UserOffset = UserTimeZone.GetUtcOffset(now);

            var hours = 0;
            var minutes = 0;

            hours = UserOffset.Hours;
            minutes = UserOffset.Minutes;

            var timeSpan = (hours * 2) + (minutes / 30);


            var timeRangeArr = UTCTimeRange.Split(',');

            var timeRangeConvertedArr = new string[timeRangeArr.Length];

            for (int i = 0; i < timeRangeArr.Length; i++)
            {
                timeRangeConvertedArr[i] = "0";
            }

            if (timeSpan == 0) //Don't shift
            {
                timeRangeConvertedArr = timeRangeArr;
            }
            else if (timeSpan < 0) //Shift Left
            {
                //Array.Copy(timeRangeArr, 0, timeRangeConvertedArr, timeSpan, timeRangeArr.Length - timeSpan);
                timeRangeConvertedArr = Shift(timeRangeArr, -(timeSpan));
            }
            else //Shift Right
            {
                //Array.Copy(timeRangeArr, 0, timeRangeConvertedArr, timeSpan, timeRangeArr.Length - timeSpan);
                timeRangeConvertedArr = Shift(timeRangeArr, timeSpan);
            }

            return string.Join(",", timeRangeConvertedArr);
        }

        public T[] Shift<T>(T[] array, int shiftValue)
        {
            var newArray = new T[array.Length];
            shiftValue -= array.Length;
            if (shiftValue < 0)
            {
                shiftValue *= -1;
            }


            for (var i = 0; i < array.Length; i++)
            {
                var index = (i + shiftValue) % array.Length;

                newArray[i] = array[index];
            }
            return newArray;
        }
    }
}
