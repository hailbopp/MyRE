public static String version() { return "v0.0.1" }

public static String handle() { return "FreeCoRE" }

definition(
    name: handle(),
    namespace: "zabracks",
    author: "Drew Worthey",
    description: "Automate all the things!",
    category: "Convenience",
    singleInstance: true,
    iconUrl: "https://s3.amazonaws.com/smartapp-icons/Convenience/Cat-Convenience.png",
    iconX2Url: "https://s3.amazonaws.com/smartapp-icons/Convenience/Cat-Convenience@2x.png",
    iconX3Url: "https://s3.amazonaws.com/smartapp-icons/Convenience/Cat-Convenience@2x.png"
)

preferences {
    page(name: "pageTopLevel")
    page(name: "pageWebConsole")
    page(name: "pageDeviceSelect")
    page(name: "pageFinishInstall")
}

def pageTopLevel() {
	def endpoint = getHubEndpoint()
	if (!state.installed) {
        return dynamicPage(name: "pageTopLevel", title: "", install: false, uninstall: false, nextPage: "pageWebConsole") {
            section() {
                paragraph "Welcome to ${handle()}"
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
	}
    dynamicPage(name: "pageTopLevel", title: "", install: true, uninstall: false) {
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

private sectionDomainEntry() {
    section() {
        input "apiDomain", "text", title: "Enter the domain of the ${handle()} server that you wish to use.", defaultValue: "http://freecoreweb.azurewebsites.net", required: true
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
    return dynamicPage(name: 'pageWebConsole', title: "", nextPage:'pageDeviceSelect') {
        if (!state.installed) {
            if (endpoint) {
                if (hasTimeZone) {
                    section() {
                        paragraph "Name this ${handle()} instance."
                        label name: "name", title: "Name", state: (name ? "complete" : null), defaultValue: app.name, required: false
                    }

                    section() {
                        paragraph "${state.installed ? "Tap Done to continue." : "Enter the password that you will use to authenticate yourself."}", required: false
                    }
                }
            } else {
                section() {
                    paragraph "OAuth is not enabled. After it has been enabled, retry.", required: true
                }
                return
            }
        }
        sectionDomainEntry()
        sectionPasswordEntry()
    }
}

private pageDeviceSelect() {
	state.deviceVersion = now().toString()
	dynamicPage(name: "pageDeviceSelect", title: "", nextPage: "pageFinishInstall") {
		section() {
			paragraph "${state.installed ? "Select the devices you want ${handle()} to have access to." : "Great, now let's select some devices."}"
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
            paragraph "Now tap Done and enjoy ${handle()}!"
		}
	}
}

private void initTokens() {
	state.securityTokens = [:]
}

/// Handlers
def primaryHandler(event) {
    if (!event || (!event.name.endsWith(handle()))) return;
    def data = event.jsonData ?: null

}

/// Subscription management
private subscribeToEvents() {
    subscribe(location, "${handle()}.poll", primaryHandler)
	subscribe(location, "${'@@' + handle()}", primaryHandler)
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

/// Utilities
private info(message, shift = null, err = null) { debug message, shift, err, 'info' }
private trace(message, shift = null, err = null) { debug message, shift, err, 'trace' }
private warn(message, shift = null, err = null) { debug message, shift, err, 'warn' }
private error(message, shift = null, err = null) { debug message, shift, err, 'error' }
private timer(message, shift = null, err = null) { debug message, shift, err, 'timer' }

/// Data

private static Map capabilities() {
    //n = name
    //d = friendly devices name
    //a = default attribute
    //c = accepted commands
    //m = momentary
    //s = number of subdevices
    //i = subdevice index in event data
	return [
		accelerationSensor			: [ n: "Acceleration Sensor",			d: "acceleration sensors",			a: "acceleration",																																																							],
		actuator					: [ n: "Actuator", 						d: "actuators",																																																																	],
		alarm						: [ n: "Alarm",							d: "alarms and sirens",				a: "alarm",								c: ["off", "strobe", "siren", "both"],																																								],
		audioNotification			: [ n: "Audio Notification",			d: "audio notification devices",											c: ["playText", "playTextAndResume", "playTextAndRestore", "playTrack", "playTrackAndResume", "playTrackAndRestore"],				 																],
		battery						: [ n: "Battery",						d: "battery powered devices",		a: "battery",																																																								],
		beacon						: [ n: "Beacon",						d: "beacons",						a: "presence",																																																								],
		bulb						: [ n: "Bulb",							d: "bulbs",							a: "switch",							c: ["off", "on"],																																													],
		button						: [ n: "Button",						d: "buttons",						a: "button",				m: true,	s: "numberOfButtons,numButtons", i: "buttonNumber",																																					],
		carbonDioxideMeasurement	: [ n: "Carbon Dioxide Measurement",	d: "carbon dioxide sensors",		a: "carbonDioxide",																																																							],
		carbonMonoxideDetector		: [ n: "Carbon Monoxide Detector",		d: "carbon monoxide detectors",		a: "carbonMonoxide",																																																						],
		colorControl				: [ n: "Color Control",					d: "adjustable color lights",		a: "color",								c: ["setColor", "setHue", "setSaturation"],																																							],
		colorTemperature			: [ n: "Color Temperature",				d: "adjustable white lights",		a: "colorTemperature",					c: ["setColorTemperature"],																																											],
		configuration				: [ n: "Configuration",					d: "configurable devices",													c: ["configure"],																																													],
		consumable					: [ n: "Consumable",					d: "consumables",					a: "consumableStatus",					c: ["setConsumableStatus"],																																											],
		contactSensor				: [ n: "Contact Sensor",				d: "contact sensors",				a: "contact",																																																								],
		doorControl					: [ n: "Door Control",					d: "automatic doors",				a: "door",								c: ["close", "open"],																																												],
		energyMeter					: [ n: "Energy Meter",					d: "energy meters",					a: "energy",																																																								],
		estimatedTimeOfArrival		: [ n: "Estimated Time of Arrival", 	d: "moving devices (ETA)",			a: "eta",																																																									],
		garageDoorControl			: [ n: "Garage Door Control",			d: "automatic garage doors",		a: "door",								c: ["close", "open"],																																												],
		holdableButton				: [ n: "Holdable Button",				d: "holdable buttons",				a: "button",				m: true,	s: "numberOfButtons,numButtons", i: "buttonNumber",																																					],
		illuminanceMeasurement		: [ n: "Illuminance Measurement",		d: "illuminance sensors",			a: "illuminance",																																																							],
		imageCapture				: [ n: "Image Capture",					d: "cameras, imaging devices",		a: "image",								c: ["take"],																																														],
		indicator					: [ n: "Indicator",						d: "indicator devices",				a: "indicatorStatus",					c: ["indicatorNever", "indicatorWhenOn", "indicatorWhenOff"],																																		],
		infraredLevel				: [ n: "Infrared Level",				d: "adjustable infrared lights",	a: "infraredLevel",						c: ["setInfraredLevel"],																																											],
		light						: [ n: "Light",							d: "lights",						a: "switch",							c: ["off", "on"],																		 																											],
		lock						: [ n: "Lock",							d: "electronic locks",				a: "lock",								c: ["lock", "unlock"],	s:"numberOfCodes,numCodes", i: "usedCode", 																									 								],
		lockOnly					: [ n: "Lock Only",						d: "electronic locks (lock only)",	a: "lock",								c: ["lock"],																																														],
		mediaController				: [ n: "Media Controller",				d: "media controllers",				a: "currentActivity",					c: ["startActivity", "getAllActivities", "getCurrentActivity"],																																		],
		momentary					: [ n: "Momentary",						d: "momentary switches",													c: ["push"],																																														],
		motionSensor				: [ n: "Motion Sensor",					d: "motion sensors",				a: "motion",																																																								],
        musicPlayer					: [ n: "Music Player",					d: "music players",					a: "status",							c: ["mute", "nextTrack", "pause", "play", "playTrack", "previousTrack", "restoreTrack", "resumeTrack", "setLevel", "setTrack", "stop", "unmute"],													],
		notification				: [ n: "Notification",					d: "notification devices",													c: ["deviceNotification"],																																											],
		outlet						: [ n: "Outlet",						d: "lights",						a: "switch",							c: ["off", "on"],																																										 			],
		pHMeasurement				: [ n: "pH Measurement",				d: "pH sensors",					a: "pH",																																																									],
        polling						: [ n: "Polling",						d: "pollable devices",														c: ["poll"],																																														],
		powerMeter					: [ n: "Power Meter",					d: "power meters",					a: "power",																																																									],
		powerSource					: [ n: "Power Source",					d: "multisource powered devices",	a: "powerSource",																																																							],
		presenceSensor				: [ n: "Presence Sensor",				d: "presence sensors",				a: "presence",																																																								],
		refresh						: [ n: "Refresh",						d: "refreshable devices",													c: ["refresh"],																																														],
		relativeHumidityMeasurement	: [ n: "Relative Humidity Measurement",	d: "humidity sensors",				a: "humidity",																																																								],
		relaySwitch					: [ n: "Relay Switch",					d: "relay switches",				a: "switch",							c: ["off", "on"],																																													],
		sensor						: [ n: "Sensor",						d: "sensors",						a: "sensor",																																																								],
		shockSensor					: [ n: "Shock Sensor",					d: "shock sensors",					a: "shock",																																																									],
		signalStrength				: [ n: "Signal Strength",				d: "wireless devices",				a: "rssi",																																																									],
		sleepSensor					: [ n: "Sleep Sensor",					d: "sleep sensors",					a: "sleeping",																																																								],
		smokeDetector				: [ n: "Smoke Detector",				d: "smoke detectors",				a: "smoke",																																																									],
		soundPressureLevel			: [ n: "Sound Pressure Level",			d: "sound pressure sensors",		a: "soundPressureLevel",																																																					],
		soundSensor					: [ n: "Sound Sensor",					d: "sound sensors",					a: "sound",																																																									],
		speechRecognition			: [ n: "Speech Recognition",			d: "speech recognition devices",	a: "phraseSpoken",			m: true,																																																		],
		speechSynthesis				: [ n: "Speech Synthesis",				d: "speech synthesizers",													c: ["speak"],																																														],
		stepSensor					: [ n: "Step Sensor",					d: "step counters",					a: "steps",																																																									],
		switch						: [ n: "Switch",						d: "switches",						a: "switch",							c: ["off", "on"],																																										 			],
		switchLevel					: [ n: "Switch Level",					d: "dimmers and dimmable lights",	a: "level",								c: ["setLevel"],																																													],
		tamperAlert					: [ n: "Tamper Alert",					d: "tamper sensors",				a: "tamper",																																																								],
		temperatureMeasurement		: [ n: "Temperature Measurement",		d: "temperature sensors",			a: "temperature",																																																							],
		thermostat					: [ n: "Thermostat",					d: "thermostats",					a: "thermostatMode",					c: ["auto", "cool", "emergencyHeat", "fanAuto", "fanCirculate", "fanOn", "heat", "off", "setCoolingSetpoint", "setHeatingSetpoint", "setSchedule", "setThermostatFanMode", "setThermostatMode"],	],
		thermostatCoolingSetpoint	: [ n: "Thermostat Cooling Setpoint",	d: "thermostats (cooling)",			a: "coolingSetpoint",					c: ["setCoolingSetpoint"],																																											],
		thermostatFanMode			: [ n: "Thermostat Fan Mode",			d: "fans",							a: "thermostatFanMode",					c: ["fanAuto", "fanCirculate", "fanOn", "setThermostatFanMode"],																																	],
		thermostatHeatingSetpoint	: [ n: "Thermostat Heating Setpoint",	d: "thermostats (heating)",			a: "heatingSetpoint",					c: ["setHeatingSetpoint"],																																											],
		thermostatMode				: [ n: "Thermostat Mode",													a: "thermostatMode",					c: ["auto", "cool", "emergencyHeat", "heat", "off", "setThermostatMode"],																															],
		thermostatOperatingState	: [ n: "Thermostat Operating State",										a: "thermostatOperatingState",																																																				],
		thermostatSetpoint			: [ n: "Thermostat Setpoint",												a: "thermostatSetpoint",																																																					],
		threeAxis					: [ n: "Three Axis Sensor",				d: "three axis sensors",			a: "orientation",																																																							],
		timedSession				: [ n: "Timed Session",					d: "timers",						a: "sessionStatus",						c: ["cancel", "pause", "setTimeRemaining", "start", "stop", ],																																		],
		tone						: [ n: "Tone",							d: "tone generators",														c: ["beep"],																																														],
		touchSensor					: [ n: "Touch Sensor",					d: "touch sensors",					a: "touch",																																																									],
		ultravioletIndex			: [ n: "Ultraviolet Index",				d: "ultraviolet sensors",			a: "ultravioletIndex",																																																						],
		valve						: [ n: "Valve",							d: "valves",						a: "valve",								c: ["close", "open"],																																												],
		voltageMeasurement			: [ n: "Voltage Measurement",			d: "voltmeters",					a: "voltage",																																																								],
		waterSensor					: [ n: "Water Sensor",					d: "water and leak sensors",		a: "water",																																																									],
		windowShade					: [ n: "Window Shade",					d: "automatic window shades",		a: "windowShade",						c: ["close", "open", "presetPosition"],																																								],
	]
}
