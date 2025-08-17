/** consts */
import viewRouteAccountConsts from "../consts/viewRouteAccountConsts";
import viewRouteAttendanceConsts from "../consts/viewRouteAttendanceConsts";

/** components */
import { createBrowserRouter } from "react-router-dom";
import PrivateRoutes from "../features/App/components/PrivateRoutes"
import NotFound from "../features/App/NotFound";
import MainLayout from "../features/App/components/MainLayout";
import Login from "../features/account/Login";
import Logout from "../features/account/Logout";
import UserCreate from "../features/account/UserCreate";
import AttendanceMenu from "../features/attendance/AttendanceMenu";
import AttendanceList from "../features/attendance/AttendanceList";
import AttendanceEdit from "../features/attendance/AttendanceEdit";

/**  
 * ルーター定義コンポーネント
 * NOTE:BrowserRouterはRouteが親子関係状態で子を表示すると親のコンポーネントもページに表示される
 *      Outletの位置に子が表示される
 *  */
const router = createBrowserRouter([
    {
        element: <MainLayout />,
        children: [

            /* アカウント機能（ルートはログイン画面とする）*/
            {
                path: viewRouteAccountConsts.Login,
                element: <Login />,
            },
            {
                path: viewRouteAccountConsts.Logout,
                element: <Logout />,
            },
            {
                path: viewRouteAccountConsts.UserCreate,
                element: <UserCreate />,
            },

            /* 勤怠メニュー */
            {
                path: viewRouteAttendanceConsts.AttendanceMenu,
                element: <PrivateRoutes><AttendanceMenu /></PrivateRoutes>,
            },
            {
                path: viewRouteAttendanceConsts.AttendanceList,
                element: <PrivateRoutes><AttendanceList /></PrivateRoutes>,
            },
            {
                path: viewRouteAttendanceConsts.AttendanceEdit,
                element: <PrivateRoutes><AttendanceEdit /></PrivateRoutes>,
            },

            /* 不正アクセス */
            {
                path: '*',
                element: <NotFound />
            }
        ]
    }
]);

/** エクスポート */
export default router;