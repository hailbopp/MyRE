public static String version() { return "v0.0.1" }

def APP_NAME = "FreeCoRE"

definition(
    name: APP_NAME,
    namespace: "zabracks",
    author: "Drew Worthey",
    description: "Automate all the things!",
    category: "Convenience",
    singleInstance: false,
    iconUrl: "https://s3.amazonaws.com/smartapp-icons/Convenience/Cat-Convenience.png",
    iconX2Url: "https://s3.amazonaws.com/smartapp-icons/Convenience/Cat-Convenience@2x.png",
    iconX3Url: "https://s3.amazonaws.com/smartapp-icons/Convenience/Cat-Convenience@2x.png"
)

def pageToplevel() {
	def endpoint = getHubEndpoint()
	if (!state.installed) {
        return dynamicPage(name: "pageToplevel", title: "", install: false, uninstall: false, nextPage: "pageWebConsole") {
            section() {
                paragraph "Welcome to ${APP_NAME}"
                paragraph "You will be guided through a few installation steps that should only take a minute."
            }
            if (endpoint) {
               	if (!location.getTimeZone()) {
 	               section() {
	                	paragraph "Please set up your location."
                   }
                } else {
                    section() {
                        paragraph "Let's continue to the next step."
                    }
                }
            } else {
                section() {
                    paragraph "OAuth needs to be configured for this SmartApp in the SmartThings IDE."
                }
	            state.oAuthRequired = true
                section () {
                    paragraph "Once you have finished the steps above, tap Next", required: true
                }
            }
        }
	} else {
        return dynamicPage(name: "pageToplevel", title: "", install: true, uninstall: false) {
            section("Management Console") {
                if (!state.endpoint) {
                    href "pageWebConsole", title: "Management Console", description: "Tap to initialize", image: "https://s3.amazonaws.com/smartapp-icons/Convenience/Cat-Convenience.png", required: false
                } else {
                    //trace "*** DO NOT SHARE THIS LINK WITH ANYONE *** Dashboard URL: ${getDashboardInitUrl()}"
                    href "", title: "Management Console", style: "external", url: getDashboardInitUrl(), description: "Tap to open", image: "https://s3.amazonaws.com/smartapp-icons/Convenience/Cat-Convenience.png", required: false
                    href "", title: "Authenticate a client", style: "embedded", url: getDashboardInitUrl(true), description: "Tap to open", image: "https://s3.amazonaws.com/smartapp-icons/Convenience/Cat-Convenience.png", required: false
                }
            }

            section(title:"Settings") {
                href "pageSettings", title: "Settings", image: "https://cdn.rawgit.com/ady624/${handle()}/master/resources/icons/settings.png", required: false
            }

        }
    }
}

private sectionDomainEntry() {
    section() {
        input "apiDomain", "text", title: "Enter the domain of the ${APP_NAME} server that you wish to use.", required: true
    }
}

private sectionPasswordEntry() {
    section() {
        input "password", "password", title: "Enter a password for your management console.", required: true
        input "expiry", "enum", options: ["Every hour", "Every day", "Every week", "Every month (recommended)", "Every three months", "Never (not recommended)"], defaultValue: "Every month (recommended)", title: "Choose how often to require reauthentication", required: true
    }
}

private pageWebConsole() {
    def endpoint = getHubEndpoint()
    def hasTimeZone = !!location.getTimeZone()
    return dynamicPage(name: 'pageWebConsole', title: "", nextPage: endpoint && hasTimeZone ? 'pageDeviceSelect' : null) {
        if (!state.installed) {
            if (endpoint) {
                if (hasTimeZone) {
                    section() {
                        paragraph "Name this ${APP_NAME} instance."
                        label name: "name", title: "Name", state: (name ? "complete" : null), defaultValue: app.name, required: false
                    }
                    section() {
                        paragraph "${state.installed ? "Tap Done to continue." : "Continue to enter the password that you will use to authenticate yourself."}", required: false
                    }
                }
            } else {
                section() {
                    paragraph "OAuth is not enabled. After it has been enabled, retry.", required: true
                }
                return
            }
        }
        sectionPasswordEntry()
    }
}

private pageDeviceSelect() {
	state.deviceVersion = now().toString()
	dynamicPage(name: "pageDeviceSelect", title: "", nextPage: state.installed ? null : "pageFinishInstall") {
		section() {
			paragraph "${state.installed ? "Select the devices you want ${APP_NAME} to have access to." : "Great, now let's select some devices."}"
        }

		section ('Select devices by type') {
        	paragraph "Most devices should fall into one of these two categories"
			input "dev:actuator", "capability.actuator", multiple: true, title: "Which actuators", required: false
			input "dev:sensor", "capability.sensor", multiple: true, title: "Which sensors", required: false
		}

		section ('Select devices by capability') {
        	paragraph "If you cannot find a device by type, you may try looking for it by category below"
			for (capability in capabilities().findAll{ (!(it.value.d in [null, 'actuators', 'sensors'])) }.sort{ it.value.d }) {
				if (capability.value.d != d) input "dev:${capability.key}", "capability.${capability.key}", multiple: true, title: "Which ${capability.value.d}", required: false
				d = capability.value.d
			}
		}
	}
}

private pageFinishInstall() {
	initTokens()
	dynamicPage(name: "pageFinishInstall", title: "", install: true) {
		section() {
			paragraph "Installation is complete."
        }
        section("Note") {
            paragraph "After you tap Done, go to the Automation tab, select the SmartApps section, and open the SmartApp to access the management console.", required: true
            paragraph "You can also access the console on any another device by entering REPLACE_WITH_DOMAIN in the address bar of your browser.", required: true
        }
        section() {
            paragraph "Now tap Done and enjoy ${APP_NAME}!"
		}
	}
}

private void initTokens() {
	state.securityTokens = [:]
}

/// Handlers
def primaryHandler(event) {
    if (!event || (!event.name.endsWith(APP_NAME))) return;
    def data = event.jsonData ?: null

}

/// Subscription management
private subscribeToEvents() {
    subscribe(location, "${APP_NAME}.poll", primaryHandler)
	subscribe(location, "${'@@' + APP_NAME}", primaryHandler)
    //subscribe(location, "HubUpdated", hubUpdatedHandler, [filterEvents: false])
    //subscribe(location, "summary", summaryHandler, [filterEvents: false])
    //setPowerSource(getHub()?.isBatteryInUse() ? 'battery' : 'mains')
}

/// Registration with server
def registerInstance() {
    //TODO
}

/// Initialization
def installed() {
	state.installed = true
	initialize()
	return true
}

def updated() {
	unsubscribe()
    unschedule()
	initialize()
    return true
}

private initialize() {
    subscribeToEvents()    
    state.vars = state.vars ?: [:]
    state.version = version()
    if (state.installed && settings.agreement) {
    	registerInstance()
    }
}

private getHubEndpoint() {
    if (!state.endpoint) {
        try {
            def accessToken = createAccessToken()
            if (accessToken) {
                state.endpoint = hubUID ? apiServerUrl("$hubUID/apps/${app.id}/?access_token=${state.accessToken}") : apiServerUrl("/api/token/${accessToken}/smartapps/installations/${app.id}/")
            }
        } catch(e) {
            state.endpoint = null
        }
    }
    return state.endpoint
}