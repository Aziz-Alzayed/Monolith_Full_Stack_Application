namespace FSTD.DataCore.Models.JwtModels
{
    public class JwtRefreshModel
    {
        // can be used for usage tracking
        // can optionally include other metadata, such as user agent, ip address, device name, and so on
        public string UserEmail { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpireAt { get; set; }
    }
}
