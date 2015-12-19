using OmniSharp.Common;

namespace OmniSharp.Build
{
    public class BuildRequest : Request
    {
        private bool _useDevenv = true;

        public bool UseDevenv
        {
            get { return _useDevenv; }
            set { _useDevenv = value; }
        }
    }
}

