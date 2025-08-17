/** css */
import "./css/Loading.css";

/** provider */
import { useLoading } from "../provider/LoadingProvider";

/**
 * ヘッダー
 * @param userModel ユーザー情報
 * @returns コンポーネント
 */
const Loading = () => {
    const { isLoading } = useLoading();

    return isLoading ? (
        <>
            <div id="loading">
                <div className="text-center position-absolute top-50 start-50 w-100 translate-middle">
                    <div className="spinner spinner-border text-success" role="status" />
                </div>
            </div>
        </>
    ) : null;
};

/** エクスポート */
export default Loading;