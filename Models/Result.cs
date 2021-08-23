using System;
using System.Collections.Generic;

namespace OSKI_Solutions_Test.Models
{
    public class Result
    {
        public List<ResultEntity> ResultList;

    }

    public class ResultEntity
    {
        public Guid passed_test;
        public bool IsCorrect;
    }
}
