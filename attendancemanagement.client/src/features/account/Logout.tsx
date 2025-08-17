/** npm */
import { useEffect, useRef } from "react";
import { useNavigate } from "react-router-dom";

/** consts */
import ViewRouteAccountConsts from "../../consts/viewRouteAccountConsts";

/** provider */
import { useLoading } from "../../provider/LoadingProvider";
import { useAccount } from '../../provider/AccountProvider';

/** types */
import { type UserModel } from "../../types/UserModel";
import AccountLogic from "./logic/accountLogic";

/**
 * ログアウト画面
 * @param setUserModel ユーザー情報
 * @returns コンポーネント
 */
const Logout = () => {

    const navigate = useNavigate();
    const { setLoading } = useLoading();
    const isFirstRender = useRef(true);
    const { setUser } = useAccount();

    const logic = new AccountLogic();

    useEffect(() => {
        if (isFirstRender.current) {

            /** スピナー表示 */
            setLoading(true);

            /** StrictMode対策 */
            isFirstRender.current = false;

            // 同期処理
            const onActionLogout = async () => {
                try {
                    await logic.Logout();

                    const userModel: UserModel = {
                        userName: ""
                    };

                    setUser(userModel);

                } catch (error) {
                    console.error(error);
                } finally {
                    setLoading(false);
                    navigate(ViewRouteAccountConsts.Login);
                }
            };

            void onActionLogout();
        }
    }, []);

    return (
        <></>
    );
}

/** エクスポート */
export default Logout;