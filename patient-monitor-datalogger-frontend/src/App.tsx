import { Routes, Route } from "react-router";
import { Layout } from "./components/Layout";
import HomePage from "./pages/HomePage";

const App = () => {

  return (<Routes>
    <Route path='/' element={<Layout />}>
        <Route index element={<HomePage />} />
    </Route>
  </Routes>);
}

export default App
