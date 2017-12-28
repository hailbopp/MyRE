
export namespace Store {
    export interface Nav {
        navPaneOpen: boolean;
    }

    export interface Script {
        name: string;

    }

    export interface All {
        nav: Nav;
    }
}