import objectInitializerUtil from './default-converter.js';

(function() {
    let rootObjectNameElement = document.getElementById('root-object-name');
    let converterTypeElement = document.getElementById('converter-type');
    let jsonContentElement = document.getElementById('json-content');
    let createObjectInitializerBtn = document.getElementById('generate-object-initializer');
    let resetBtn = document.getElementById('reset');
    let output = document.getElementById('output');
    let outputBlock = document.getElementById('output-block');
    let errorText = document.getElementById('error-text');
    let errorBlock = document.getElementById('error');

    const setOutput = (displayType, message) => {
        output.innerText = message;
        outputBlock.style.display = displayType;
    }

    const setError = (displayType, message) => {
        errorText.innerText = message;
        errorBlock.style.display = displayType;
        setOutput('none', '');
    }

    const resetElements = () => {
        rootObjectNameElement.value = '';
        converterTypeElement.value = '';
        jsonContentElement.value = '';
    }

    const isValidObjectName = (rootObjectName) => {
        const titlePattern = /([A-Z][a-z]+)+/;
        return titlePattern.test(rootObjectName);
    }

    createObjectInitializerBtn.addEventListener('click', function() {
        const rootObjectName = rootObjectNameElement.value;
        const converterType = converterTypeElement.value;
        const jsonContent = jsonContentElement.value;
        const isValidName = isValidObjectName(rootObjectName);
        if (!isValidName) {
            const message = 'Root Object Name should be in a PascalCase.';
            setError('block', message);
            return;
        }

        if (!jsonContent) {
            const message = 'JSON content is empty.';
            setError('block', message);
            return;
        }

        setError('none', '');
        let extractedData = '';
        try {
            if (converterType === 'Default') {
                extractedData = objectInitializerUtil(jsonContent, rootObjectName);
            } else if (converterType === 'JToken') {
                extractedData = objectInitializerUtil(jsonContent, rootObjectName);
            } else if (converterType === 'Typed') {
                extractedData = objectInitializerUtil(jsonContent, rootObjectName);
            } else {
                const message = 'Invalid Converter Type.';
                setError('block', message);
                return;
            }
        } catch(error) {
            setError('block', error);
            return;
        }

        setOutput('block', extractedData);
        resetElements();
    });

    resetBtn.addEventListener('click', function() {
        setOutput('none', '');
    });
})();
