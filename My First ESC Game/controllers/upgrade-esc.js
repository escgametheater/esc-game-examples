#!/usr/bin/env node
const fs = require("fs");
const child_process = require('child_process');
const package = require("./package.json");
const node_exec = child_process.exec;

const exec = (cmd, opts={}) =>
	new Promise((resolve, reject) => {
		node_exec(cmd, opts, (err) => {
			if (err) reject(err)
			else resolve()
		})
	});

const writeFile = (path, data, opts = "utf8") =>
	new Promise((resolve, reject) => {
		fs.writeFile(path, data, opts, (err) => {
			if (err) reject(err)
			else resolve()
		})
	});

const escDependencies =
	Object.keys(package.devDependencies)
		.filter(d => !!~d.indexOf("@esc_games"));

if (!escDependencies.length) {
	console.error("Unable to find any @esc_games dependencies in devDependencies");
	process.exit(1);
}

console.log("Found: ", escDependencies);

(async () => {
	console.log("Installing latest versions of those dependencies");
	const command = `npm i ${escDependencies.join("@latest ")} --save-dev`;
	await exec(command);

	// NPM will overwrite @latest with the resolved package version, we want latest
	escDependencies.forEach(d => {
		package.devDependencies[d] = "latest";
	});

	await writeFile("./package.json", JSON.stringify(package, null, 4));

	console.log("Done");
})();