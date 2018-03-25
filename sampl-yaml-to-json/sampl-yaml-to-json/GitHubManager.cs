using Octokit;

namespace sampl_yaml_to_json
{
    /// <summary>
    /// Manages a GitHub singleton to help provide access to GitHub APIs from anywhere in the code.
    /// </summary>
    class GitHubManager
    {
        private static GitHubManager _instance;
        private static string _authtoken = "";
        private static GitHubClient _client;

        private GitHubManager()
        {
            _client = new GitHubClient(new Octokit.ProductHeaderValue("sample-yaml-bulk-updater"));
        }

        public static GitHubManager instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GitHubManager();
                }
                return _instance;
            }
        }
        public string authtoken
        {
            set
            {
                _authtoken = value;
                if (_instance == null)
                {
                    _instance = new GitHubManager();
                }
                else
                {
                    _client.Credentials = new Credentials(_authtoken);
                }
            }
        }
        public GitHubClient client
        {
            get
            {
                return _client;
            }
        }
    }
}