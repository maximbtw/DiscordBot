using System.Collections.Generic;
using ClosedXML.Excel;

namespace DiscordExcel
{
    
    public class ExcelParser
    {
        private static readonly string path = "Расписание.xlsx";
        public Timetable Timetable { get; private set; }

        private IXLWorksheet workSheet;
        private Cell cellGroup;

        public bool WritingExcel(string nameGroup)
        {
            var workBook = new XLWorkbook(path);
            workSheet = workBook.Worksheet("Лист1");

            cellGroup = FindCellGroup(nameGroup);

            if (cellGroup.Empty()) return false;

            Timetable = new Timetable(nameGroup);

            foreach (var cellDayOfWeak in FindDayOfWeak())
                foreach (var cellTime in FindTime(cellDayOfWeak))
                    FindSubject(cellTime);

            return true;
        }

        private Cell FindCellGroup(string nameGroup)
        {
            Cell cell;
            for (int y = 1; ; y++)
            {
                string message = workSheet.Cell(y, 1).Value.ToString();
                if (string.IsNullOrEmpty(message)) continue;
                if (message.ToLower().Equals("группы"))
                {
                    cell.Y = y;
                    break;
                }
            }

            for (int x = 1; ; x++)
            {
                if (x == 1000) return new Cell(0, 0);
                string message = workSheet.Cell(cell.Y, x).Value.ToString();
                if (string.IsNullOrEmpty(message)) continue;
                if (message.ToLower().Equals(nameGroup))
                {
                    cell.X = x;
                    break;
                }
            }

            return cell;
        }

        private DayOfTheWeak currentDay;
        private Time currentTime;

        private IEnumerable<Cell> FindDayOfWeak()
        {
            for (int y = cellGroup.Y + 1; ; y++)
            {
                string message = workSheet.Cell(y, 1).Value.ToString(); ;
                if (!string.IsNullOrEmpty(message))
                {
                    currentDay = new DayOfTheWeak(message);
                    Timetable.dayOfWeeks.Add(currentDay);
                    yield return new Cell(y, 1);
                    if (message.ToLower().Equals("пятница")) yield break;
                }
            }
        }

        private IEnumerable<Cell> FindTime(Cell cellDayOfWeak)
        {
            for (int y = cellDayOfWeak.Y; y < cellDayOfWeak.Y + 20; y++)
            {
                string message = workSheet.Cell(y, 2).Value.ToString(); ;
                if (!string.IsNullOrEmpty(message))
                {
                    currentTime = new Time(message);
                    currentDay.Times.Add(currentTime);
                    yield return new Cell(y, 2);
                }
            }
        }

        private void FindSubject(Cell cellTime)
        {
            var parityWeak = CheckTheWeak(new Cell(cellTime.Y, cellGroup.X));
            var subject = new Subject();
            var text = string.Empty;

            for (int y = cellTime.Y; y < cellTime.Y + 4; y++)
            {
                var cell = MoveCellToRight(new Cell(y, cellGroup.X));
                if (parityWeak)
                {
                    if (cellTime.Y == y)
                    {
                        subject.EvenWeak.ContainsSubject = false;
                    }
                    if (cellTime.Y + 2 == y)
                    {
                        AddSubject(subject, text);
                        subject = new Subject();
                        text = string.Empty;
                        subject.OddWeak.ContainsSubject = false;
                    }
                }

                string message = workSheet.Cell(cell.Y, cell.X).Value.ToString();
                if (!string.IsNullOrEmpty(message))
                {
                    text += string.IsNullOrEmpty(text) ? message : "\n" + message;
                }
            }

            AddSubject(subject, text);
        }

        private void AddSubject(Subject subject, string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                subject.TextSubject = text;
                currentTime.Subjects.Add(subject);
            }
        }

        private Cell MoveCellToRight(Cell cell)
        {
            while (true)
            {
                var convertCell = workSheet.Cell(cell.Y, cell.X);

                if (convertCell.Style.Border.LeftBorder.Equals(XLBorderStyleValues.None))
                {
                    cell.X = cell.X - 1;
                }
                else
                {
                    return cell;
                }
            }
        }

        private bool CheckTheWeak(Cell cell)
        {
            return !workSheet.Cell(cell.Y + 2, cell.X)
                   .Style.Border.TopBorder
                   .Equals(XLBorderStyleValues.None)
                   ||
                   !workSheet.Cell(cell.Y + 1, cell.X)
                   .Style.Border.BottomBorder
                   .Equals(XLBorderStyleValues.None);
        }

        private struct Cell
        {
            public int X;
            public int Y;

            public Cell(int y, int x) { Y = y; X = x; }

            public bool Empty()
            {
                return X == 0 && Y == 0;
            }
        }
    }
}

