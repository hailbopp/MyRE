type OpenNavPaneAction = {
    type: 'NAV_PANE_OPEN',
}

type CloseNavPaneAction = {
    type: 'NAV_PANE_CLOSE',
}

export const openNavPane = (): OpenNavPaneAction => ({
    type: 'NAV_PANE_OPEN'
})

export const closeNavPane = (): CloseNavPaneAction => ({
    type: 'NAV_PANE_CLOSE'
})

export type NavAction =
    | OpenNavPaneAction
    | CloseNavPaneAction;