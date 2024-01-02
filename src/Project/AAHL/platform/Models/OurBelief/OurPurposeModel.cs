using Glass.Mapper.Sc.Fields;

namespace Project.AAHL.Website.Models.OurBelief
{
    public class OurPurposeModel
    {
        public virtual string Heading { get; set; }
        public virtual string Description { get; set; }
        public virtual Image ImagePath { get; set; }
        public virtual Image MImagePath { get; set; }
        public virtual Image TImagePath { get; set; }
    }
}