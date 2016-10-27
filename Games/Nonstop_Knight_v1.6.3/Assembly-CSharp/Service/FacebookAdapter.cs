namespace Service
{
    using App;
    using Facebook.Unity;
    using GameLogic;
    using PlayerView;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class FacebookAdapter : MonoBehaviour
    {
        [CompilerGenerated]
        private Dictionary<string, FbPlatformUser> <Friends>k__BackingField;
        [CompilerGenerated]
        private FbPlatformUser <Identity>k__BackingField;
        [CompilerGenerated]
        private bool <Initialized>k__BackingField;
        private List<string> m_facebookPermissions;
        private bool? m_fbinit;
        private Coroutine m_imageLoader;
        private List<LeaderboardEntry> m_leaderboard;
        private bool? m_loadedFriends;
        private bool? m_loadedMe;
        private bool? m_loggedIn;

        public FacebookAdapter()
        {
            List<string> list = new List<string>();
            list.Add("public_profile");
            list.Add("user_friends");
            this.m_facebookPermissions = list;
        }

        private void AddOwnLeaderboardEntry()
        {
            if (this.Identity != null)
            {
                Player player = GameLogic.Binder.GameState.Player;
                string str = !(player.SocialData.Name == _.L(ConfigLoca.HERO_KNIGHT, null, false)) ? player.SocialData.Name : this.Identity.userName;
                LeaderboardEntry entry2 = new LeaderboardEntry();
                entry2.UserId = this.Identity.id;
                entry2.Name = str;
                entry2.IsSelf = true;
                entry2.HighestFloor = GameLogic.Binder.GameState.Player.getHighestFloorReached();
                entry2.ImageTexture = this.Identity.image;
                LeaderboardEntry item = entry2;
                item.setDefaultPlayerHeroAvatarSprite();
                this.m_leaderboard.Add(item);
            }
        }

        private void Awake()
        {
            if (ConfigApp.CHEAT_MARKETING_MODE_ENABLED)
            {
                this.m_fbinit = true;
            }
            else
            {
                this.m_fbinit = null;
                if (!FB.IsInitialized)
                {
                    InitDelegate delegate2;
                    if (ConfigApp.ProductionBuild)
                    {
                        delegate2 = new InitDelegate(this.onInitComplete);
                        FB.Init(ConfigApp.FacebookAppIdProduction, true, false, true, false, true, null, "en_US", null, delegate2);
                    }
                    else
                    {
                        delegate2 = new InitDelegate(this.onInitComplete);
                        FB.Init(ConfigApp.FacebookAppIdDevelopment, true, true, true, false, true, null, "en_US", null, delegate2);
                    }
                }
                else
                {
                    this.m_fbinit = true;
                    FB.ActivateApp();
                }
            }
        }

        private FbPlatformUser createFbUser(IDictionary<string, object> entry, [Optional, DefaultParameterValue(null)] FbPlatformUser user)
        {
            if (user == null)
            {
            }
            user = new FbPlatformUser();
            string[] keys = new string[] { "id" };
            user.id = this.getValueForKeys<string>(entry, keys);
            string[] textArray2 = new string[] { "name" };
            user.userName = this.getValueForKeys<string>(entry, textArray2);
            string[] textArray3 = new string[] { "picture", "data", "is_silhouette" };
            if (!this.getValueForKeys<bool>(entry, textArray3))
            {
                string[] textArray4 = new string[] { "picture", "data", "url" };
                user.ImageUrl = this.getValueForKeys<string>(entry, textArray4);
            }
            return user;
        }

        public LeaderboardEntry GetEntry(int idx)
        {
            if (this.IsLoggedIn() && (this.m_leaderboard.Count < idx))
            {
                return this.m_leaderboard[idx];
            }
            return null;
        }

        public IList<FbPlatformUser> GetFriends()
        {
            if (!this.IsLoggedIn())
            {
                return null;
            }
            IList<FbPlatformUser> list = new List<FbPlatformUser>();
            foreach (FbPlatformUser user in Enumerable.ToList<FbPlatformUser>(this.Friends.Values))
            {
                if (user.id != this.Identity.id)
                {
                    list.Add(user);
                }
            }
            return list;
        }

        public LeaderboardEntry GetMyEntry()
        {
            if (this.IsLoggedIn())
            {
                foreach (LeaderboardEntry entry in this.m_leaderboard)
                {
                    if (entry.UserId == this.Identity.id)
                    {
                        return entry;
                    }
                }
            }
            return null;
        }

        private T getValueForKeys<T>(IDictionary<string, object> dict, params string[] keys)
        {
            object obj3;
            for (int i = 0; i < (keys.Length - 1); i++)
            {
                object obj2;
                if (dict.TryGetValue(keys[i], out obj2))
                {
                    if (obj2 is IDictionary<string, object>)
                    {
                        dict = (IDictionary<string, object>) obj2;
                    }
                }
                else
                {
                    return default(T);
                }
            }
            if (dict.TryGetValue(keys[keys.Length - 1], out obj3) && (obj3 is T))
            {
                return (T) obj3;
            }
            return default(T);
        }

        private void InitDummyLeaderboard()
        {
            this.m_leaderboard.Clear();
            this.m_leaderboard.AddRange(ConfigLeaderboard.DUMMY_PLAYERS);
            this.AddOwnLeaderboardEntry();
            App.Binder.EventBus.LeaderboardLoaded(LeaderboardType.Royal);
        }

        [DebuggerHidden]
        public IEnumerator Initialize()
        {
            <Initialize>c__Iterator21C iteratorc = new <Initialize>c__Iterator21C();
            iteratorc.<>f__this = this;
            return iteratorc;
        }

        private void InitUnconnectedContent()
        {
            if (this.Friends != null)
            {
                this.Friends.Clear();
            }
            this.m_leaderboard.Clear();
            this.m_leaderboard.AddRange(ConfigLeaderboard.DUMMY_PLAYERS);
            Player player = GameLogic.Binder.GameState.Player;
            FbPlatformUser user = new FbPlatformUser();
            user.userName = _.L(ConfigLoca.LEADERBOARD_YOU, null, false);
            user.id = "unauthenticated_local_player_leaderboard_user";
            this.Identity = user;
            LeaderboardEntry entry2 = new LeaderboardEntry();
            entry2.Name = _.L(ConfigLoca.LEADERBOARD_YOU, null, false);
            entry2.IsSelf = true;
            entry2.UserId = "unauthenticated_local_player_leaderboard_user";
            entry2.HighestFloor = player.getHighestFloorReached();
            LeaderboardEntry item = entry2;
            item.setDefaultPlayerHeroAvatarSprite();
            this.m_leaderboard.Add(item);
            App.Binder.EventBus.LeaderboardLoaded(LeaderboardType.Royal);
        }

        public bool IsLoggedIn()
        {
            return (!FB.IsLoggedIn ? false : (!this.m_loggedIn.HasValue ? false : this.m_loggedIn.Value));
        }

        [DebuggerHidden]
        private IEnumerator LoadFriends()
        {
            <LoadFriends>c__Iterator221 iterator = new <LoadFriends>c__Iterator221();
            iterator.<>f__this = this;
            return iterator;
        }

        [DebuggerHidden]
        private IEnumerator loadImages(List<LeaderboardImage> lbImages, List<LeaderboardEntry> lbEntries)
        {
            <loadImages>c__Iterator222 iterator = new <loadImages>c__Iterator222();
            iterator.lbEntries = lbEntries;
            iterator.lbImages = lbImages;
            iterator.<$>lbEntries = lbEntries;
            iterator.<$>lbImages = lbImages;
            iterator.<>f__this = this;
            return iterator;
        }

        [DebuggerHidden]
        private IEnumerator LoadMe()
        {
            <LoadMe>c__Iterator220 iterator = new <LoadMe>c__Iterator220();
            iterator.<>f__this = this;
            return iterator;
        }

        [DebuggerHidden]
        private IEnumerator loadMissingImageTextures(List<LeaderboardImage> lbImages, List<FbPlatformUser> fbUsers)
        {
            <loadMissingImageTextures>c__Iterator223 iterator = new <loadMissingImageTextures>c__Iterator223();
            iterator.fbUsers = fbUsers;
            iterator.lbImages = lbImages;
            iterator.<$>fbUsers = fbUsers;
            iterator.<$>lbImages = lbImages;
            iterator.<>f__this = this;
            return iterator;
        }

        [DebuggerHidden]
        private IEnumerator LoadPicture(FbPlatformUser fbUser)
        {
            <LoadPicture>c__Iterator224 iterator = new <LoadPicture>c__Iterator224();
            iterator.fbUser = fbUser;
            iterator.<$>fbUser = fbUser;
            return iterator;
        }

        [DebuggerHidden]
        private IEnumerator LoadProfiles()
        {
            <LoadProfiles>c__Iterator21D iteratord = new <LoadProfiles>c__Iterator21D();
            iteratord.<>f__this = this;
            return iteratord;
        }

        private void Log(string str)
        {
            Service.Binder.Logger.Log(str);
        }

        [DebuggerHidden]
        public IEnumerator Login(string context)
        {
            <Login>c__Iterator21F iteratorf = new <Login>c__Iterator21F();
            iteratorf.context = context;
            iteratorf.<$>context = context;
            iteratorf.<>f__this = this;
            return iteratorf;
        }

        public void Logout(string context)
        {
            if (FB.IsLoggedIn)
            {
                FB.LogOut();
                this.InitUnconnectedContent();
                this.m_loggedIn = false;
                this.TrackFacebookEvent("logout", "login", context);
            }
        }

        public void MergeFriendsInLeaderboard(List<LeaderboardEntry> leaderboard)
        {
            bool flag = false;
            this.m_leaderboard.Clear();
            foreach (LeaderboardEntry entry in leaderboard)
            {
                string str = !(entry.Name == _.L(ConfigLoca.HERO_KNIGHT, null, false)) ? entry.Name : null;
                if (entry.UserId == this.Identity.id)
                {
                    entry.IsSelf = true;
                    if (str == null)
                    {
                    }
                    entry.Name = this.Identity.userName;
                    entry.HighestFloor = GameLogic.Binder.GameState.Player.getHighestFloorReached();
                    flag = true;
                    this.m_leaderboard.Add(entry);
                }
                else
                {
                    FbPlatformUser user;
                    if (this.Friends.TryGetValue(entry.UserId, out user))
                    {
                        if (str == null)
                        {
                        }
                        entry.Name = user.userName;
                        entry.ImageTexture = user.image;
                        this.m_leaderboard.Add(entry);
                    }
                }
            }
            if (!flag)
            {
                this.AddOwnLeaderboardEntry();
            }
            this.m_leaderboard.AddRange(ConfigLeaderboard.DUMMY_PLAYERS);
            this.Log("populated leaderboard with " + this.Leaderboard.Count);
            App.Binder.EventBus.LeaderboardLoaded(LeaderboardType.Royal);
        }

        private void onInitComplete()
        {
            if (FB.IsInitialized)
            {
                FB.ActivateApp();
                this.m_fbinit = true;
            }
            else
            {
                this.m_fbinit = false;
                this.Log("Failed to initialize Facebook SDK");
            }
        }

        private void OnLoggedIn(IResult loginResult)
        {
            if (loginResult.Cancelled)
            {
                this.m_loggedIn = false;
            }
            else if (loginResult.Error != null)
            {
                this.m_loggedIn = false;
            }
            else
            {
                this.m_loggedIn = true;
            }
        }

        private void OnPostResult(IGraphResult result)
        {
            this.Log("On Post result" + result.Error);
        }

        private void OnReadFriends(IGraphResult result)
        {
            this.Log("OnReadFriends:" + JsonUtils.Serialize(result));
            if (!result.Cancelled && (result.Error == null))
            {
                string[] keys = new string[] { "data" };
                IEnumerator<object> enumerator = this.getValueForKeys<IList<object>>(result.ResultDictionary, keys).GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        IDictionary<string, object> current = (IDictionary<string, object>) enumerator.Current;
                        string[] textArray2 = new string[] { "id" };
                        string key = this.getValueForKeys<string>(current, textArray2);
                        FbPlatformUser user = null;
                        this.Friends.TryGetValue(key, out user);
                        user = this.createFbUser(current, user);
                        user.isFriend = true;
                        this.Friends[key] = user;
                    }
                }
                finally
                {
                    if (enumerator == null)
                    {
                    }
                    enumerator.Dispose();
                }
                this.m_loadedFriends = true;
                App.Binder.EventBus.PlatformFriendsUpdated(PlatformConnectType.Facebook);
            }
            else
            {
                this.m_loadedFriends = false;
            }
            this.Log("Friends:" + this.Friends.Count);
        }

        private void OnReadMe(IGraphResult result)
        {
            this.Log("OnReadMe:" + JsonUtils.Serialize(result));
            if (!result.Cancelled && (result.Error == null))
            {
                this.Identity = this.createFbUser(result.ResultDictionary, null);
                this.Identity.isFriend = false;
                this.Friends[this.Identity.id] = this.Identity;
                this.m_loadedMe = true;
            }
            else
            {
                this.m_loadedMe = false;
            }
        }

        private void OnTokenRefresh(IAccessTokenRefreshResult result)
        {
            this.Log("Refresh token result " + result.RawResult);
            if (!FB.IsLoggedIn)
            {
                App.Binder.EventBus.PlatformConnectStateChanged(PlatformConnectType.Facebook);
                this.m_loggedIn = false;
            }
            else if (result.Error != null)
            {
                this.m_loggedIn = false;
            }
            else
            {
                this.m_loggedIn = true;
            }
        }

        public void PopulateImages(List<LeaderboardImage> lbImages, List<LeaderboardEntry> lbEntries)
        {
            if (this.m_imageLoader != null)
            {
                Service.Binder.TaskManager.StopCoroutine(this.m_imageLoader);
            }
            this.m_imageLoader = Service.Binder.TaskManager.StartTask(this.loadImages(lbImages, lbEntries), null);
        }

        public void PopulateImages(List<LeaderboardImage> lbImages, List<FbPlatformUser> fbUsers)
        {
            if (this.m_imageLoader != null)
            {
                Service.Binder.TaskManager.StopCoroutine(this.m_imageLoader);
            }
            this.m_imageLoader = Service.Binder.TaskManager.StartTask(this.loadMissingImageTextures(lbImages, fbUsers), null);
        }

        [DebuggerHidden]
        private IEnumerator RefreshAccessToken()
        {
            <RefreshAccessToken>c__Iterator21E iteratore = new <RefreshAccessToken>c__Iterator21E();
            iteratore.<>f__this = this;
            return iteratore;
        }

        public bool RequiresUserConnect()
        {
            return !FB.IsLoggedIn;
        }

        private void TrackFacebookEvent(string action, string flow, string context)
        {
            if (Service.Binder.TrackingSystem != null)
            {
                Player player = GameLogic.Binder.GameState.Player;
                if (context == null)
                {
                }
                Service.Binder.TrackingSystem.sendFacebookEvent(player, action, flow, "unknown");
            }
        }

        public Dictionary<string, FbPlatformUser> Friends
        {
            [CompilerGenerated]
            get
            {
                return this.<Friends>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Friends>k__BackingField = value;
            }
        }

        public FbPlatformUser Identity
        {
            [CompilerGenerated]
            get
            {
                return this.<Identity>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Identity>k__BackingField = value;
            }
        }

        public bool Initialized
        {
            [CompilerGenerated]
            get
            {
                return this.<Initialized>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Initialized>k__BackingField = value;
            }
        }

        public List<LeaderboardEntry> Leaderboard
        {
            get
            {
                return this.m_leaderboard;
            }
        }

        [CompilerGenerated]
        private sealed class <Initialize>c__Iterator21C : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal FacebookAdapter <>f__this;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 0:
                        this.<>f__this.Log("Initialize FacebookAdapter");
                        this.<>f__this.Friends = new Dictionary<string, FbPlatformUser>();
                        this.<>f__this.m_leaderboard = new List<LeaderboardEntry>();
                        this.<>f__this.Initialized = false;
                        break;

                    case 1:
                        break;

                    case 2:
                        goto Label_00A8;

                    case 3:
                        this.$current = this.<>f__this.LoadProfiles();
                        this.$PC = 4;
                        goto Label_0100;

                    case 4:
                        this.$PC = -1;
                        goto Label_00FE;

                    default:
                        goto Label_00FE;
                }
                if (GameLogic.Binder.GameState.Player == null)
                {
                    this.$current = null;
                    this.$PC = 1;
                    goto Label_0100;
                }
            Label_00A8:
                while (!this.<>f__this.m_fbinit.HasValue)
                {
                    this.$current = null;
                    this.$PC = 2;
                    goto Label_0100;
                }
                this.$current = this.<>f__this.RefreshAccessToken();
                this.$PC = 3;
                goto Label_0100;
            Label_00FE:
                return false;
            Label_0100:
                return true;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            object IEnumerator<object>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }
        }

        [CompilerGenerated]
        private sealed class <LoadFriends>c__Iterator221 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal FacebookAdapter <>f__this;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 0:
                        this.<>f__this.m_loadedFriends = null;
                        FB.API("me/friends?fields=name,picture&limit=100", HttpMethod.GET, new FacebookDelegate<IGraphResult>(this.<>f__this.OnReadFriends), (IDictionary<string, string>) null);
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_0086;
                }
                if (!this.<>f__this.m_loadedFriends.HasValue)
                {
                    this.$current = null;
                    this.$PC = 1;
                    return true;
                }
                this.$PC = -1;
            Label_0086:
                return false;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            object IEnumerator<object>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }
        }

        [CompilerGenerated]
        private sealed class <loadImages>c__Iterator222 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal List<LeaderboardEntry> <$>lbEntries;
            internal List<LeaderboardImage> <$>lbImages;
            internal FacebookAdapter <>f__this;
            internal int <k>__0;
            internal FbPlatformUser <person>__1;
            internal List<LeaderboardEntry> lbEntries;
            internal List<LeaderboardImage> lbImages;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 0:
                        this.<k>__0 = 0;
                        goto Label_00DC;

                    case 1:
                        if (this.<person>__1.image != null)
                        {
                            this.lbImages[this.<k>__0].refresh(this.lbEntries[this.<k>__0].AvatarSpriteId, this.<person>__1.image);
                        }
                        break;

                    default:
                        goto Label_0105;
                }
            Label_00CE:
                this.<k>__0++;
            Label_00DC:
                if (this.<k>__0 < this.lbEntries.Count)
                {
                    if (this.<>f__this.Friends.TryGetValue(this.lbEntries[this.<k>__0].UserId, out this.<person>__1))
                    {
                        this.$current = this.<>f__this.LoadPicture(this.<person>__1);
                        this.$PC = 1;
                        return true;
                    }
                    goto Label_00CE;
                }
                this.<>f__this.m_imageLoader = null;
                this.$PC = -1;
            Label_0105:
                return false;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            object IEnumerator<object>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }
        }

        [CompilerGenerated]
        private sealed class <LoadMe>c__Iterator220 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal FacebookAdapter <>f__this;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 0:
                        this.<>f__this.m_loadedMe = null;
                        FB.API("me?fields=name,picture", HttpMethod.GET, new FacebookDelegate<IGraphResult>(this.<>f__this.OnReadMe), (IDictionary<string, string>) null);
                        break;

                    case 1:
                        break;

                    case 2:
                        goto Label_00C0;

                    default:
                        goto Label_00C7;
                }
                if (!this.<>f__this.m_loadedMe.HasValue)
                {
                    this.$current = null;
                    this.$PC = 1;
                    goto Label_00C9;
                }
                if (this.<>f__this.m_loadedMe.Value)
                {
                    this.$current = this.<>f__this.LoadPicture(this.<>f__this.Identity);
                    this.$PC = 2;
                    goto Label_00C9;
                }
            Label_00C0:
                this.$PC = -1;
            Label_00C7:
                return false;
            Label_00C9:
                return true;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            object IEnumerator<object>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }
        }

        [CompilerGenerated]
        private sealed class <loadMissingImageTextures>c__Iterator223 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal List<FbPlatformUser> <$>fbUsers;
            internal List<LeaderboardImage> <$>lbImages;
            internal FacebookAdapter <>f__this;
            internal int <k>__0;
            internal FbPlatformUser <person>__1;
            internal List<FbPlatformUser> fbUsers;
            internal List<LeaderboardImage> lbImages;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 0:
                        this.<k>__0 = 0;
                        break;

                    case 1:
                        if (this.<person>__1.image != null)
                        {
                            this.lbImages[this.<k>__0].refresh(null, this.<person>__1.image);
                        }
                        this.<k>__0++;
                        break;

                    default:
                        goto Label_00D6;
                }
                if (this.<k>__0 < this.fbUsers.Count)
                {
                    this.<person>__1 = this.fbUsers[this.<k>__0];
                    this.$current = this.<>f__this.LoadPicture(this.<person>__1);
                    this.$PC = 1;
                    return true;
                }
                this.<>f__this.m_imageLoader = null;
                this.$PC = -1;
            Label_00D6:
                return false;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            object IEnumerator<object>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }
        }

        [CompilerGenerated]
        private sealed class <LoadPicture>c__Iterator224 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal FbPlatformUser <$>fbUser;
            internal WWW <www>__0;
            internal FbPlatformUser fbUser;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 0:
                        if ((this.fbUser.ImageUrl != null) && (this.fbUser.image == null))
                        {
                            this.<www>__0 = new WWW(this.fbUser.ImageUrl);
                            this.$current = this.<www>__0;
                            this.$PC = 1;
                            return true;
                        }
                        break;

                    case 1:
                        if (this.<www>__0.error == null)
                        {
                            this.fbUser.image = this.<www>__0.texture;
                        }
                        this.$PC = -1;
                        break;
                }
                return false;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            object IEnumerator<object>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }
        }

        [CompilerGenerated]
        private sealed class <LoadProfiles>c__Iterator21D : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal FacebookAdapter <>f__this;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 0:
                        if (!FB.IsLoggedIn || !this.<>f__this.m_loggedIn.Value)
                        {
                            this.<>f__this.Log("Loading profiles skipped, not logged in");
                            this.<>f__this.InitUnconnectedContent();
                            break;
                        }
                        this.<>f__this.Log("Loading profiles");
                        this.$current = this.<>f__this.LoadMe();
                        this.$PC = 1;
                        goto Label_014E;

                    case 1:
                        this.$current = this.<>f__this.LoadFriends();
                        this.$PC = 2;
                        goto Label_014E;

                    case 2:
                        this.<>f__this.InitDummyLeaderboard();
                        if (this.<>f__this.m_loadedMe.Value && this.<>f__this.m_loadedFriends.Value)
                        {
                            if (this.<>f__this.m_loadedMe.Value)
                            {
                                App.Binder.EventBus.PlatformConnectStateChanged(PlatformConnectType.Facebook);
                                App.Binder.EventBus.PlatformProfileUpdated(PlatformConnectType.Facebook, this.<>f__this.Identity);
                            }
                            break;
                        }
                        this.<>f__this.Log("not all data could be loaded, using dummy content");
                        this.<>f__this.InitUnconnectedContent();
                        break;

                    default:
                        goto Label_014C;
                }
                this.<>f__this.Initialized = true;
                this.$PC = -1;
            Label_014C:
                return false;
            Label_014E:
                return true;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            object IEnumerator<object>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }
        }

        [CompilerGenerated]
        private sealed class <Login>c__Iterator21F : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal string <$>context;
            internal FacebookAdapter <>f__this;
            internal string context;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 0:
                        if (!FB.IsInitialized)
                        {
                            goto Label_0144;
                        }
                        this.<>f__this.TrackFacebookEvent("click", "login", this.context);
                        this.<>f__this.m_loggedIn = null;
                        FB.LogInWithReadPermissions(this.<>f__this.m_facebookPermissions, new FacebookDelegate<ILoginResult>(this.<>f__this.OnLoggedIn));
                        break;

                    case 1:
                        break;

                    case 2:
                        goto Label_00C8;

                    case 3:
                        goto Label_0144;

                    default:
                        goto Label_014B;
                }
                if (!this.<>f__this.m_loggedIn.HasValue)
                {
                    this.$current = null;
                    this.$PC = 1;
                    goto Label_014D;
                }
            Label_00C8:
                while (PlayerView.Binder.MenuSystem.InTransition)
                {
                    this.$current = null;
                    this.$PC = 2;
                    goto Label_014D;
                }
                if (this.<>f__this.m_loggedIn.Value)
                {
                    this.<>f__this.TrackFacebookEvent("login", "login", this.context);
                    this.$current = this.<>f__this.LoadProfiles();
                    this.$PC = 3;
                    goto Label_014D;
                }
                this.<>f__this.TrackFacebookEvent("login_failed", "login", this.context);
            Label_0144:
                this.$PC = -1;
            Label_014B:
                return false;
            Label_014D:
                return true;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            object IEnumerator<object>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }
        }

        [CompilerGenerated]
        private sealed class <RefreshAccessToken>c__Iterator21E : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal FacebookAdapter <>f__this;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 0:
                        if (!FB.IsLoggedIn)
                        {
                            this.<>f__this.m_loggedIn = false;
                            goto Label_00A8;
                        }
                        this.<>f__this.Log("Refresh token");
                        this.<>f__this.m_loggedIn = null;
                        FB.Mobile.RefreshCurrentAccessToken(new FacebookDelegate<IAccessTokenRefreshResult>(this.<>f__this.OnTokenRefresh));
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_00AF;
                }
                if (!this.<>f__this.m_loggedIn.HasValue)
                {
                    this.$current = null;
                    this.$PC = 1;
                    return true;
                }
            Label_00A8:
                this.$PC = -1;
            Label_00AF:
                return false;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            object IEnumerator<object>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }
        }
    }
}

