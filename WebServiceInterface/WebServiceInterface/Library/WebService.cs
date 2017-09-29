namespace WebServiceInterface.Library
{
    class WebService
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public Method[] Methods { get; set; }
    }
}
