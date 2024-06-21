import fs from 'node:fs/promises';
import os from 'node:os';
import { fileURLToPath } from 'url';
import * as path from 'path';

const __filename = fileURLToPath(import.meta.url);
const __dirname = path.dirname(__filename);
const envFilePath = path.resolve(__dirname, ".env");
const readEnvVars = async () => fs.readFile(envFilePath, "utf-8")

/**
 * Finds the key in .env files and returns the corresponding value
 *
 * @param {string} key Key to find
 * @returns {string|null} Value of the key
 */
const getEnvValue = (key) => {
    // find the line that contains the key (exact match)
    const matchedLine = readEnvVars().find((line) => line.split("=")[0] === key);
    // split the line (delimiter is '=') and return the item at index 2
    return matchedLine !== undefined ? matchedLine.split("=")[1] : null;
};

/**
 * Updates value for existing key or creates a new key=value line
 *
 * This function is a modified version of https://stackoverflow.com/a/65001580/3153583
 *
 * @param {string} key Key to update/insert
 * @param {string} value Value to update/insert
 */
const setEnvValue = async (key, value) => {
    const envVars = (await readEnvVars()).split(os.EOL);

    const targetLine = envVars.find((line) => line.split("=")[0] === key);
    if (targetLine !== undefined) {
        // update existing line
        const targetLineIndex = envVars.indexOf(targetLine);
        // replace the key/value with the new value
        envVars.splice(targetLineIndex, 1, `${key}="${value}"`);
    } else {
        // create new key value
        envVars.push(`${key}="${value}"`);
    }
    // write everything back to the file system
    await fs.writeFile(envFilePath, envVars.join(os.EOL));
};



try {
    const data = await fs.readFile('../Backend/Properties/launchSettings.json', { encoding: 'utf8' });
    const json = JSON.parse(data);
    setEnvValue('VITE_BASEURL', json.iisSettings.iisExpress.applicationUrl)
} catch (err) {
    console.log(err);
}