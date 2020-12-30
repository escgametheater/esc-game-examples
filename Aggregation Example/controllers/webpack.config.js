const path = require("path");
const fs = require("fs");

let packageLockJson = require("./package-lock.json");
if (
  !packageLockJson ||
  !packageLockJson.dependencies["@esc_games/esc-controller-sdk"]
) {
  packageLockJson.dependencies["@esc_games/esc-controller-sdk"] = {
    version: "linked-local"
  };
}

const escSdkVersion =
  packageLockJson.dependencies["@esc_games/esc-controller-sdk"].version;
const configurator = require("@esc_games/esc-controller-build-configurator")(
  __dirname,
  escSdkVersion
);

if (!process.env.CONTROLLER) {
  console.error("CONTROLLER not found in ENV");
  process.exit(1);
}

const entry = path.resolve(
  __dirname,
  `./src/main/${process.env.CONTROLLER}/index.js`
);
const outputPath = path.resolve(__dirname, `./dist/${process.env.CONTROLLER}`);

if (!fs.existsSync(entry)) {
	console.log(`Unable to find a controller at ${entry}, skipping.`);
	process.exit(0);
}

module.exports = (env, argv) => {
  const config = configurator(env, argv);
  config.entry = entry;
  config.output.path = outputPath;
  config.resolve.alias = Object.assign(config.resolve.alias || {}, {
    Root: path.resolve(__dirname, "./src"),
    Lib: path.resolve(__dirname, "./src/lib"),
    Main: path.resolve(__dirname, "./src/main")
  });
  return config;
};
