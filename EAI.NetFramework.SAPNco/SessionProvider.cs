using SAP.Middleware.Connector;
using System;
using System.Collections.Generic;
using System.Threading;

namespace EAI.NetFramework.SAPNco
{
    public class SessionProvider : ISessionProvider
    {
        private static SessionProvider _instance = new SessionProvider();
        public static SessionProvider Instance { get { return _instance; } }

        static SessionProvider()
        {
            RfcSessionManager.RegisterSessionProvider(Instance);
        }

        private Dictionary<string, RfcSession> _sessions = new Dictionary<string, RfcSession>();
        private int _sessionCounter = 0;
        private ThreadLocal<string> _currentSession = new ThreadLocal<string>();

        internal void SetCurrentSession(string sessionId)
        {
            lock (_sessions)
                if (!_sessions.ContainsKey(sessionId))
                    throw new RfcInvalidSessionException(sessionId);

            _currentSession.Value = sessionId;
        }

        public event RfcSessionManager.SessionChangeHandler SessionChanged;

        public bool ChangeEventsSupported()
        {
            return true;
        }

        public void ContextFinished()
        {
        }

        public void ContextStarted()
        {
        }

        public string CreateSession()
        {
            var session = new RfcSession(Interlocked.Increment(ref _sessionCounter).ToString());

            lock (_sessions)
                _sessions.Add(session.SessionId, session);

            _currentSession.Value = session.SessionId;

            return session.SessionId;
        }

        public void DestroySession(string sessionId)
        {
            lock (_sessions)
                _sessions.Remove(sessionId);

            if(_currentSession.Value == sessionId)
                _currentSession.Value = null;

            SessionChanged?.Invoke(new RfcSessionEventArgs(RfcSessionManager.EventType.DESTROYED, sessionId));
        }

        public string GetCurrentSession()
        {
            var sessionId = _currentSession.Value;
            if (sessionId == null)
                return null;

            lock (_sessions)
            {
                if (!_sessions.TryGetValue(sessionId, out RfcSession session))
                    throw new RfcInvalidSessionException(sessionId);

                return sessionId;
            }
        }

        public bool IsAlive(string sessionId)
        {
            lock (_sessions)
                return _sessions.ContainsKey(sessionId);
        }

        public void ActivateSession(string sessionId)
        {
            lock (_sessions)
            {
                if (!_sessions.TryGetValue(sessionId, out RfcSession session))
                    throw new RfcInvalidSessionException(sessionId);

                session.IsActive = true;
            }
        }

        public void PassivateSession(string sessionId)
        {
            lock (_sessions)
            {
                if (!_sessions.TryGetValue(sessionId, out RfcSession session))
                    throw new RfcInvalidSessionException(sessionId);

                session.IsActive = false;
            }
        }
    }
}
