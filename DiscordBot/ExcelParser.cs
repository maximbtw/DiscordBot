using System;
using System.Collections.Generic;
using ClosedXML.Excel;
using DiscordExcel;

namespace DiscordBot
{
    public class ExcelParser
    {
        public Timetable Timetable { get; private set; }
        
        private const string Path = "Расписание.xlsx";

        private readonly IXLWorksheet _workSheet;
        private readonly string _nameGroup;
        private readonly Cell _cellGroup;
        
        private DayOfTheWeak _currentDay;
        private Time _currentTime;

        public ExcelParser(string nameGroup)
        {
            const string nameSheet = "Лист1";
            
            var workBook = new XLWorkbook(Path);
            
            _nameGroup = nameGroup;
            _workSheet = workBook.Worksheet(nameSheet);
            _cellGroup = FindCellGroup();
        }
        
        public bool TryWriteExcelDocument() => !_cellGroup.Empty();

        public void Parse()
        {
            if (_cellGroup.Empty())
            {
                throw new NullReferenceException(message: "Cell group is null");
            }
            
            Timetable = new Timetable(_nameGroup);

            foreach (var cellDayOfWeak in FindDayOfWeak())
            {
                foreach (var cellTime in FindTime(cellDayOfWeak))
                {
                    FindSubject(cellTime);
                }
            }
        }

        private Cell FindCellGroup()
        {
            var cell = new Cell();
            
            if (TryFindYCell(ref cell))
            {
                if (TryFindXCell(ref cell))
                {
                    return cell;
                }
            }

            return new Cell(0, 0);
        }

        private bool TryFindYCell(ref Cell cell)
        {
            int y = 1;
            
            while (y < int.MaxValue)
            {
                string message = _workSheet.Cell(y, 1).Value.ToString();

                bool isFind = !string.IsNullOrEmpty(message) && message.ToLower().Equals("группы");
                    
                if (isFind)
                {
                    cell.Y = y;

                    return true;
                }

                y++;
            }

            return false;
        }
        
        private bool TryFindXCell(ref Cell cell)
        {
            int x = 1;
            
            while (x < 1000)
            {
                string message = _workSheet.Cell(cell.Y, x).Value.ToString();

                bool isFind = !string.IsNullOrEmpty(message) && message.ToLower().Equals(_nameGroup);

                if (isFind)
                {
                    cell.X = x;

                    return true;
                }

                x++;
            }

            return false;
        }
        
        private IEnumerable<Cell> FindDayOfWeak()
        {
            const string lastDayOfWeekName = "пятница";
            
            for (int y = _cellGroup.Y + 1; ; y++)
            {
                string cellText = _workSheet.Cell(y, 1).Value.ToString();
                
                if (!string.IsNullOrEmpty(cellText))
                {
                    _currentDay = new DayOfTheWeak(cellText);
                    Timetable.DayOfWeeks.Add(_currentDay);
                    
                    yield return new Cell(y, 1);

                    if (cellText.ToLower().Equals(lastDayOfWeekName))
                    {
                        yield break;
                    }
                }
            }
        }

        private IEnumerable<Cell> FindTime(Cell cellDayOfWeak)
        {
            for (int y = cellDayOfWeak.Y; y < cellDayOfWeak.Y + 20; y++)
            {
                string cellText = _workSheet.Cell(y, 2).Value.ToString();
                
                if (!string.IsNullOrEmpty(cellText))
                {
                    _currentTime = new Time(cellText);
                    _currentDay.Times.Add(_currentTime);
                    
                    yield return new Cell(y, 2);
                }
            }
        }

        private void FindSubject(Cell cellTime)
        {
            var parityWeak = CheckTheWeak(new Cell(cellTime.Y, _cellGroup.X));
            var subject = new Subject();
            var text = string.Empty;

            for (int y = cellTime.Y; y < cellTime.Y + 4; y++)
            {
                var cell = MoveCellToRight(new Cell(y, _cellGroup.X));
                
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

                string cellText = _workSheet.Cell(cell.Y, cell.X).Value.ToString();
                
                if (!string.IsNullOrEmpty(cellText))
                {
                    text += string.IsNullOrEmpty(text) ? cellText : "\n" + cellText;
                }
            }

            AddSubject(subject, text);
        }

        private void AddSubject(Subject subject, string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                subject.TextSubject = text;
                _currentTime.Subjects.Add(subject);
            }
        }

        private Cell MoveCellToRight(Cell cell)
        {
            while (true)
            {
                IXLCell convertCell = _workSheet.Cell(cell.Y, cell.X);

                if (convertCell.Style.Border.LeftBorder.Equals(XLBorderStyleValues.None))
                {
                    cell.X -= 1;
                }
                else
                {
                    return cell;
                }
            }
        }

        private bool CheckTheWeak(Cell cell)
        {
            bool topBorderIsNone = _workSheet.Cell(cell.Y + 2, cell.X).Style.Border.TopBorder
                .Equals(XLBorderStyleValues.None);
            
            bool bottomBorderIsNone = _workSheet.Cell(cell.Y + 1, cell.X).Style.Border.BottomBorder
                .Equals(XLBorderStyleValues.None);

            return !topBorderIsNone || !bottomBorderIsNone;
        }

        private struct Cell
        {
            public int X;
            public int Y;

            public Cell(int y, int x)
            {
                Y = y; 
                X = x;
            }

            public readonly bool Empty()
            {
                return X == 0 && Y == 0;
            }
        }
    }
}

