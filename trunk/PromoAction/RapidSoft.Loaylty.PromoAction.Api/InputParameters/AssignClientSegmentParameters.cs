namespace RapidSoft.Loaylty.PromoAction.Api.InputParameters
{
    using RapidSoft.Loaylty.PromoAction.Api.DTO;

    public class AssignClientSegmentParameters
    {
        public Segment[] Segments { get; set; }

        public string UserId { get; set; }
    }
}