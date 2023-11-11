using System;

namespace ServiceLibrary
{
    public class Common
    {
        public static int GetYear(string fileName)
        {

            if (int.TryParse(fileName.Substring(fileName.LastIndexOf("\\", StringComparison.OrdinalIgnoreCase) + 1).Substring(0, 4), out int year))
            {
                return year;
            }

            throw new ArgumentOutOfRangeException();
        }

        public static int ParseTowerNumber(string fileName)
        {
            var towerNumberText = fileName.Substring(fileName.LastIndexOf("-", StringComparison.OrdinalIgnoreCase) + 1)
                .Substring(0, fileName.LastIndexOf(".", StringComparison.OrdinalIgnoreCase) - (fileName.LastIndexOf("-", StringComparison.OrdinalIgnoreCase) + 1));

            if (towerNumberText.Length == 3)
            {
                towerNumberText = $"{towerNumberText.Substring(0, 1)}0{towerNumberText.Substring(1, 2)}";
            }
            else if ((towerNumberText.Length == 4) && (towerNumberText.Substring(0, 2) == "10"))
            {
                towerNumberText = $"{towerNumberText.Substring(0, 2)}0{towerNumberText.Substring(2, 2)}";
            }

            if (int.TryParse(towerNumberText, out int towerNumber))
            {
                return towerNumber;
            }

            throw new ArgumentOutOfRangeException();
        }

        public static int FixTowerNumber(string towerNumber)
        {
            var towerNumberText = towerNumber;

            if (towerNumberText.Length == 3)
            {
                towerNumberText = $"{towerNumberText.Substring(0, 1)}0{towerNumberText.Substring(1, 2)}";
            }

            if (int.TryParse(towerNumberText, out int fixedTowerNumber))
            {
                return fixedTowerNumber;
            }

            throw new ArgumentOutOfRangeException();
        }

        public static DateTime FixDate(string date, int year)
        {
            var dateParts = date.Split(' ');

            if (DateTime.TryParse($"{dateParts[0]}/{year} {dateParts[1]}", out var fixedDate))
            {
                return fixedDate;
            }

            throw new ArgumentOutOfRangeException();
        }

        public static int ParseID(string idText)
        {
            if (string.IsNullOrWhiteSpace(idText))
            {
                return 0;
            }


            if (int.TryParse(idText, out int id))
            {
                return id;
            }

            throw new ArgumentOutOfRangeException(nameof(idText), "Invalid ID text");
        }
    }
}
