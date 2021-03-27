export function serialize(inputObject) {
    var jsonString = JSON.stringify(inputObject);
    return jsonString;
}

export function deserialize(jsonString) {
    var jsonObject = JSON.parse(jsonString);
    return jsonObject;
}

export function getPropertyName(propertyPath) {
    let propertyPathLength = propertyPath.length;
    let propertyName;
    if (propertyPath.endsWith(']')) {
        if (propertyPath[propertyPathLength - 2] == '\'') {
            // abcde['ab'] => 11-5-4
            let propertyKeyStartingIndex = propertyPath.lastIndexOf('[\'');
            propertyName = propertyPath.substr(propertyKeyStartingIndex + 2, propertyPathLength - propertyKeyStartingIndex - 4);
        } else {
            // abcde["ab"] => 11-5-4
            // to be deleted: this case won't occur
            let propertyKeyStartingIndex = propertyPath.lastIndexOf('[\"');
            propertyName = propertyPath.substr(propertyKeyStartingIndex + 2, propertyPathLength - propertyKeyStartingIndex - 4);
        }

        if (propertyName.includes('"')) {
            propertyName = propertyName.replace('\"', '\\\"');
        }
    } else {
        // abcde.ab => 8-5-1
        let propertyKeyStartingIndex = propertyPath.lastIndexOf('.');
        propertyName = propertyPath.substr(propertyKeyStartingIndex + 1, propertyPathLength - propertyKeyStartingIndex - 1);
    }

    return propertyName;
}

export function convertToPascalCase(value) {
    // value = value.toLowerCase().replace('_', ' ').replace('-', ' ');
    // let info = CultureInfo.CurrentCulture.TextInfo;
    // value = info.ToTitleCase(value).replace(' ', '');
    value = value.replace(/(\w)(\w*)/g, function(g0, g1, g2) { return g1.toUpperCase() + g2.toLowerCase(); });
    return value;
}

export function normalize(value) {
    // Removing leading digits from the value
    value = value.replace(/^[0-9]+/, '');

    // Removing unsupported chars for the property name in C#
    value = value.split('').filter((val, index) => isAlphaNumeric(val, value, index)).join('');

    // Converting value to PascalCase for the property
    value = convertToPascalCase(value);

    // Removing '_', '-', ' ' form the value
    value = value.replace(/^[0-9]+/, '');
    return value;
}

export function isAlphaNumeric(chr, str, i) {
    let code = str.charCodeAt(i);
    if (!(code > 47 && code < 58) && // numeric (0-9)
        !(code > 64 && code < 91) && // upper alpha (A-Z)
        !(code > 96 && code < 123) && // lower alpha (a-z)
        (code != 32) && // space (32)
        (code != 95) && // underscore (95)
        (code != 45) // hypen (45)
    ) {
        return false;
    }

    return true;
}

export function indent(tabIndentCount, isNewLine = true) {
    let stringBuilder = '';
    if (isNewLine) {
        for (let i = 0; i < tabIndentCount; i++) {
            stringBuilder += '\t';
        }
    }

    return stringBuilder;
}
