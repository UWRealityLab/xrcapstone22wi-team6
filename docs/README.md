# Our Blog

https://uwrealitylab.github.io/xrcapstone22wi-team6/

## Usage

First time: run `npm install`.

Edit `.pug` files in `src` (`.pug` files used to generate `.html`), run `npm run
build` to build the project in `dist`.

#### npm Scripts

You must have npm installed in order to use this build environment.

* `npm run build` builds the project - this builds assets, HTML, JS, and CSS
  into `dist`
* `npm run build:assets` copies the files in the `src/assets/` directory into
  `dist`
* `npm run build:pug` compiles the Pug located in the `src/pug/` directory into
  `dist`
* `npm run build:scripts` brings the `src/js/scripts.js` file into `dist`
* `npm run build:scss` compiles the SCSS files located in the `src/scss/`
  directory into `dist`
* `npm run clean` deletes the `dist` directory to prepare for rebuilding the
  project
* `npm run start:debug` runs the project in debug mode
* `npm start` or `npm run start` runs the project, launches a live preview in
  your default browser, and watches for changes made to files in `src`

## Copyright and License

Original template Copyright 2013-2021 Start Bootstrap LLC. Code released under
the
[MIT](https://github.com/StartBootstrap/startbootstrap-clean-blog/blob/master/LICENSE)
license.
