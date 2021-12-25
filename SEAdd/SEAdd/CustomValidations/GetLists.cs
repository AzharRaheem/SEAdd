﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SEAdd.CustomValidations
{
    public static class GetLists
    {
        public static List<string> GetGenderList()
        {
            return new List<string>() { "Male", "Female" };
        }
        public static List<string> GetMetricProgramsList()
        {
            return new List<string>() { "Arts", "Science" };
        }
        public static List<string> GetFScProgramsList()
        {
            return new List<string>() { "Pre-Engineering" , "ICS" , "DAE" };
        }
        public static List<string> GetGradesList()
        {
            return new List<string>() { "A+", "A" , "B+", "B" , "C+", "C" , "D+" , "D" , "E" , "F"};
        }
        public static List<string> GetDivisionsList()
        {
            return new List<string>() { "1st", "2nd" , "3rd" , "4th" };
        }
    }
}