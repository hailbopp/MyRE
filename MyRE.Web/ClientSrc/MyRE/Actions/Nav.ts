export type OpenNavPaneAction = {
    type: 'NAV_PANE_OPEN',
}

export type CloseNavPaneAction = {
    type: 'NAV_PANE_CLOSE',
}

export const openNavPane = (): OpenNavPaneAction => ({
    type: 'NAV_PANE_OPEN'
})

export const closeNavPane = (): CloseNavPaneAction => ({
    type: 'NAV_PANE_CLOSE'
})