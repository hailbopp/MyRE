def getNAMESPACE() { return "zabracks" }
def getPARENT_NAME() { return "MyRE" }
def getAPP_NAME() { return "${PARENT_NAME}-worker" }
def getVERSION() { return "0.0.1" }

definition(
    name: APP_NAME,
    namespace: NAMESPACE,
    author: "Drew Worthey",
    description: "MyRE worker app. Do not install directly.",
    parent: "zabracks:${PARENT_NAME}",
    category: "Convenience",
    singleInstance: false
)

preferences {
    page(name: "pageInit")
}

def pageInit() {
    return dynamicPage(name: "pageInit", title: "", uninstall: !!state.inited) {
        if(parent && parent.isInstalled()) {
            section() {
                paragraph "This is the worker app for the ${state.name} project."
            }
        } else {
            section() {
                paragraph "You cannot install this SmartApp directly. You must install the ${PARENT_NAME} app first."
            }
        }
    }
}

Boolean setup(projectId, name, description, expressionTree) {
    if(projectId && name && description && expressionTree) {
        state.projectId = projectId
        state.name = name
        state.description = description
        state.expressionTree = expressionTree

        state.inited = true

        return true
    } else {
        return false
    }
}

def installed() {
    state.created = now()
    state.modified = now()
    state.active = true
    state.variables = state.variables?: [:]
    state.subscriptions = state.subscriptions?: [:]

    return true
}

def getAllDevices() {
    return parent.getManagedDevices()
}

List<object> getExpressionChildren(node) {
    if(node.type == "LITERAL_LIST") {
        return node.value;
    } else if(node.type == "S-EXPR") {
        return [node.func].addAll(node.args)
    } //TODO: add the rest
}

def getNodeReferences(node, parentDevices) {
    def referencedDevices = []

    if(node.type == "LITERAL_REFERENCE") {
        referencedDevices.add(parentDevices.find { it.id == node.value })
    } else {
        referencedDevices.addAll(this.getExpressionChildren(node).collect {
            getNodeReferences(it, parentDevices)
        })
    }

    return referencedDevices
}

def setDeviceSubscriptions() {
    def devices = this.getAllDevices()
    def referencedDevices = []

    state.expressionTree.each { node ->
        referencedDevices.addAll(getNodeReferences(node, devices))
    }

    referencedDevices.each { d ->
        if(!state.subscriptions.contains(d.id)) {
            state.subscriptions.add(d.id)
        }
    }
}