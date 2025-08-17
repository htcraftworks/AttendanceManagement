/** css */
import "./index.css";
import "bootstrap/dist/css/bootstrap.min.css";
import "react-datepicker/dist/react-datepicker.css";

/** npm */
import React from "react";
import ReactDOM from "react-dom/client";
import { RouterProvider } from 'react-router-dom'

/** provider */
import router from './router/router.jsx'
import { AccountProvider } from "./provider/AccountProvider";
import { LoadingProvider } from "./provider/LoadingProvider";

/** components */
import Loading from "./components/Loading";

/** ルートエレメント */
const root = ReactDOM.createRoot(
    document.getElementById("root") as HTMLElement
);

/** レンダリング */
root.render(
    <React.StrictMode>
        <AccountProvider>
            <LoadingProvider>
                <RouterProvider router={router} />
                <Loading />
            </LoadingProvider>
        </AccountProvider>
    </React.StrictMode>
);