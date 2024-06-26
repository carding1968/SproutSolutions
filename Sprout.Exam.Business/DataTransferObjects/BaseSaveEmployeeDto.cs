﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Sprout.Exam.Business.DataTransferObjects
{
    public abstract class BaseSaveEmployeeDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Tin { get; set; }
        public DateTime Birthdate { get; set; }
        public int EmployeeTypeId { get; set; }
    }
}
