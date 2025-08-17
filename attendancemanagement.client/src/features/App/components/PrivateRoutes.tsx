/** npm */
import { Navigate, useLocation } from 'react-router-dom';
import { type ReactNode } from "react";

/** Consts */
import viewRouteAccountConsts from "../../../consts/viewRouteAccountConsts";

/** provider */
import { useAccount } from "../../../provider/AccountProvider";

/** Props*/
type Props = {
    children: ReactNode;
};

/**
 * 内部ルーター
 * @param param0
 * @returns
 */
const PrivateRoutes = ({ children }: Props) => {

    const { user } = useAccount();
    const location = useLocation();

    if (location.pathname != viewRouteAccountConsts.Login
        && location.pathname != viewRouteAccountConsts.Logout
        && location.pathname != viewRouteAccountConsts.UserCreate
        && location.pathname != viewRouteAccountConsts.UserCreate
        && !user.userName) {

        return <Navigate to={viewRouteAccountConsts.Login}></Navigate>;
    }

    return children;
}

/** エクスポート */
export default PrivateRoutes;