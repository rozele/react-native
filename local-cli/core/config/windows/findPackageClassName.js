const fs = require('fs');
const path = require('path');
const glob = require('glob');

/**
 * Gets package's class name (class that implements ReactPackage)
 * by searching for its declaration in all Java files present in the folder
 *
 * @param {String} folder Folder to find java files
 */
module.exports = function getPackageClassName(folder) {
  const files = glob.sync('**/*.cs', { cwd: folder });

  const packages = files
    .map(filePath => fs.readFileSync(path.join(folder, filePath), 'utf8'))
    .map(file => file.match(/class (.*) : IReactPackage/))
    .filter(match => match);

  return packages.length ? packages[0][1] : null;
};
