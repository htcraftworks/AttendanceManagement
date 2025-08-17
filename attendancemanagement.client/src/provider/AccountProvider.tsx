/** npm */
import { createContext, useContext, useState, type ReactNode } from 'react';

/** service */
import localStorageService from "../services/storage/localStorageService";
import sessionStorageService from "../services/storage/sessionStorageService";

/** types */
import { type UserModel } from "../types/UserModel";
import { type AccountContextModel } from "./types/AccountContextModel";

/** ロケール初期値 */
const LocaleInit = localStorageService.getLocale() ?? import.meta.env.VITE_APP_DEFAULT_LOCALE;

/** ユーザー情報初期値 */
const UserInit: UserModel = {
    userName: sessionStorageService.getUserName() ?? ""
};

/** Context情報 */
const AccountContext = createContext<AccountContextModel>({
    user: UserInit,
    setUser: () => { },
    locale: LocaleInit,
    setLocale: () => {}
});

/**
 * ユーザーサービス提供
 * @param props
 * @returns
 */
export const AccountProvider = ({ children }: { children: ReactNode }) => {

    const context: AccountContextModel = useContext(AccountContext);
    const [user, setUser] = useState(context.user);
    const [locale, setLocale] = useState(context.locale);

    const newContext: AccountContextModel = {
        user: user,
        setUser: (user: UserModel) => {

            if (user.userName) {
                sessionStorageService.setUserName(user.userName);
            } else {
                sessionStorageService.removeUserName();
            }

            setUser(user);
        },
        locale: locale,
        setLocale: (locale: string) => {
            /** 完璧にするならサーバと連動する必要あり */
            localStorageService.setLocale(locale)
            setLocale(locale);
        }
    };

    return (
        <AccountContext.Provider value={newContext}>
            {children}
        </AccountContext.Provider>
    );
};

/** エクスポート */
export const useAccount = () => useContext(AccountContext);