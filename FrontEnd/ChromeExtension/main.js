import { default as defaultConverter } from './default-converter.js';
import { default as jtokenConverter } from './jtoken-converter.js';
// import { default as typedConverter } from './typed-converter.js';

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

    let result = '';
    let clipboardIcon = document.getElementById('clipboard');
    let copyStatusElement = document.getElementById('copy-status');

    const setOutput = (displayType, outputCode) => {
        output.innerText = outputCode;
        result = outputCode;
        outputBlock.style.display = displayType;
    }

    const setError = (displayType, message) => {
        errorText.innerText = message;
        errorBlock.style.display = displayType;
        setOutput('none', '');
    }

    const resetElements = () => {
        rootObjectNameElement.value = '';
        converterTypeElement.value = 'Select';
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
        if (!isValidName && rootObjectName) {
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
                extractedData = defaultConverter(jsonContent, rootObjectName ? rootObjectName : undefined);
            } else if (converterType === 'JToken') {
                extractedData = jtokenConverter(jsonContent);
            } else if (converterType === 'Typed') {
                // extractedData = typedConverter(jsonContent, rootObjectName);
                const message = 'Not yet implemented';
                setError('block', message);
                return;
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

    const notifyCopySuccessStatus = () => {
        // console.log('Copied to clipboard!');
        copyStatusElement.style.display = 'inline';
        copyStatusElement.style.color = 'green';
        copyStatusElement.innerText = 'Copied!';
        setTimeout(() => {
            copyStatusElement.style.display = 'none';
            copyStatusElement.innerText = '';
       }, 3000);
    }

    const notifyCopyFailureStatus = (error) => {
        // console.error('Oops, unable to copy', error);
        // console.error('Could not copy text: ', error);
        copyStatusElement.style.display = 'inline';
        copyStatusElement.style.color = 'red';
        copyStatusElement.innerText = 'Oops, unable to copy!';
        setTimeout(() => {
            copyStatusElement.style.display = 'none';
            copyStatusElement.innerText = '';
       }, 3000);
    }

    const fallbackCopyTextToClipboard = (text) => {
        var textArea = document.createElement('textarea');
        textArea.style.whiteSpace = 'pre-wrap';
        textArea.value = text;
        
        // Avoid scrolling to bottom
        textArea.style.top = '0';
        textArea.style.left = '0';
        textArea.style.position = 'fixed';
      
        document.body.appendChild(textArea);
        textArea.focus();
        textArea.select();
      
        try {
          var successful = document.execCommand('copy');
          if (successful) {
            notifyCopySuccessStatus();
          } else {
            notifyCopyFailureStatus('');
          }
        } catch (error) {
            notifyCopyFailureStatus(error);
        }
      
        document.body.removeChild(textArea);
      }

    clipboardIcon.addEventListener('click', function() {
        if (!navigator.clipboard) {
            fallbackCopyTextToClipboard(result);
            return;
        }

        navigator.clipboard.writeText(result).then(function() {
            notifyCopySuccessStatus();
        }, function(error) {
            notifyCopyFailureStatus(error);
        });
    })
})();
