import { Routes, Route } from "react-router";
import { Layout } from "./components/Layout";
import HomePage from "./pages/HomePage";
import CreateNewLogSessionPage from "./pages/CreateNewLogSessionPage";
import { useCallback, useState } from "react";
import { Models } from "./types/models";

const App = () => {

    const [ logSessions, setLogSessions] = useState<Models.LogSession[]>([]);

    const onLogSessionCreated = useCallback((logSession: Models.LogSession) => {
        setLogSessions(state => {
            if (state.some(x => x.id === logSession.id)) {
                return state.map(x => x.id === logSession.id ? logSession : x);
            }
            return state.concat(logSession);
        });
    }, []);

    return (<Routes>
        <Route path='/' element={<Layout />}>
            <Route index element={<HomePage logSessions={logSessions} setLogSessions={setLogSessions} />} />
            <Route path="/create/session" element={<CreateNewLogSessionPage onLogSessionCreated={onLogSessionCreated} />} />
        </Route>
    </Routes>);
}

export default App
