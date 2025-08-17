/** css */
import "../css/common.css";

/** npm */
import { Outlet } from "react-router-dom";

/** components */
import Header from "./Header";
import Footer from "./Footer";

/**
 * メインレイアウト
 * @returns コンポーネント
 */
function MainLayout() {
    return (
        <>
            <Header />
            <main className="main_area">
                <Outlet />
            </main>
            <Footer />
        </>
    );
}

/** エクスポート */
export default MainLayout;