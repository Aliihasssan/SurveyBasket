using Mapster;
namespace SurveyBasket.API.Mapping
{
    public class MappingConfigurations : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            //config.NewConfig<Poll, PollResponse>().Map(
            //   des => des.FullName,
            //   scr => scr.Name + " " + scr.Description
            //    );
        }
    }
}
