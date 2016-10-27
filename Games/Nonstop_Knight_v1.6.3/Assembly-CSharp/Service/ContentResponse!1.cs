namespace Service
{
    using System;

    public class ContentResponse<T>
    {
        public string ClientUrl;
        public T Content;
        public ContentResponseType ResponseType;
    }
}

