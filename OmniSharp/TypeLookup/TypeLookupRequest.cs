using OmniSharp.Common;

namespace OmniSharp.TypeLookup
{
    public class TypeLookupRequest : Request
    {
        public bool IncludeDocumentation
        {
            get
            {
                return includeDocumentation;
            }
            set
            {
                includeDocumentation = value;
            }
        }

        public bool UseFullNames
        {
            get
            {
                return useFullNames;
            }
            set
            {
                useFullNames = value;
            }
        }

        bool useFullNames = false;
        bool includeDocumentation = true;
    }
}
