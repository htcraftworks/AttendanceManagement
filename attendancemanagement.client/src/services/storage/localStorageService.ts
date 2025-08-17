/** consts */
import LocalStorageKeyConsts from "../../consts/localStorageKeyConsts"

/** ローカルストレージ操作 */
export default class localStorageService {

    /**
     * ロケールを取得します
     */
    static getLocale(): string | null {
        return localStorageService.getLocalStorage(LocalStorageKeyConsts.LOCALE);
    }

    /**
     * ロケールを登録します
     * @param value ロケール
     */
    static setLocale(value: string): void {
        localStorageService.setLocalStorage(LocalStorageKeyConsts.LOCALE, value);
    }

    /**
     * ロケールを削除します
     * @param key キー
     */
    static removeLocale(): void {
        localStorageService.removeLocalStorage(LocalStorageKeyConsts.LOCALE);
    }

    /**
     * 認証トークンを取得します
     * @param key キー
     */
    static getAuthToken(): string | null {
        return localStorageService.getLocalStorage(LocalStorageKeyConsts.AUTH_TOKEN);
    }

    /**
     * 認証トークンを登録します
     * @param value 値
     */
    static setAuthToken(value: string): void {
        localStorageService.setLocalStorage(LocalStorageKeyConsts.AUTH_TOKEN, value);
    }

    /**
     * 認証トークンを削除します
     */
    static removeAuthToken(): void {
        localStorageService.removeLocalStorage(LocalStorageKeyConsts.AUTH_TOKEN);
    }

    /**
     * LocalStorageの対象データを取得します
     * @param key キー
     */
    private static getLocalStorage(key: string): string | null {
        return window.localStorage.getItem(key);
    }

    /**
     * LocalStorageにデータを登録します
     * @param key キー
     * @param value 値
     */
    private static setLocalStorage(key: string, value: string): void {
        window.localStorage.setItem(key, value);
    }

    /**
     * LocalStorageの対象データを削除します
     * @param key キー
     */
    private static removeLocalStorage(key: string): void {
        window.localStorage.removeItem(key);
    }
}