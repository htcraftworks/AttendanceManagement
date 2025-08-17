/** npm */
import { createContext, useContext, useState, type ReactNode } from 'react';

/** Context登録 */
const LoadingContext = createContext({
    isLoading: false,
    setLoading: (_loading: boolean) => { },
});

/**
 * ローディングサービス提供
 * @param props
 * @returns
 */
export const LoadingProvider = ({ children }: { children: ReactNode }) => {
    const [isLoading, setLoading] = useState(false);

    return (
        <LoadingContext.Provider value={{ isLoading, setLoading }}>
            {children}
        </LoadingContext.Provider>
    );
};

/** エクスポート */
export const useLoading = () => useContext(LoadingContext);