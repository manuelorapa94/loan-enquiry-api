using LoanEnquiryApi.Model.StaticReference;

namespace LoanEnquiryApi.Service
{
    public class StaticReferenceService()
    {
        public List<StaticReferenceModel> GetStaticReferences(string name)
        {
            var model = new List<StaticReferenceModel>();

            Type enumType = Type.GetType($"LoanEnquiryApi.Constant.{name}");
            if (enumType == null)
                return model;

            foreach (string enumName in Enum.GetNames(enumType))
            {
                var value = (int)Enum.Parse(enumType, enumName);

                model.Add(new StaticReferenceModel
                {
                    Value = value,
                    Name = enumName,
                });
            }

            return model;
        }
    }
}
