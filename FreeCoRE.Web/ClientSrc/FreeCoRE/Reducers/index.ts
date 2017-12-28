import { Store } from "FreeCoRE/Models/Store";
import { reduceNav } from "FreeCoRE/Reducers/Nav";

export const initialState: Store.All = {
    nav: {
        navPaneOpen: false,
    }
}

export const reduce = (state: Store.All, action: any): Store.All => ({
    nav: reduceNav(state.nav, action),
})