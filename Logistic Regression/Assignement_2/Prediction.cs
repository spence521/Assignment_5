using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignement_2
{
    public class Prediction
    {
        public int Id { get; set; }
        public int Label { get; set; }

        public Prediction(int id, int label)
        {
            Id = id;
            Label = label;
        }
    }
}
