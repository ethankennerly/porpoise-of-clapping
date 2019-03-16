# Building release

1. If release build fails:
1. Open Project Settings.
1. Select Tiny.
1. Uncheck Transpile to EcmaScript 5.
    1. Otherwise, when progress bar reaches minifying JS, console log cannot find es2017. Example:

            Build step 'Generating HTML' failed with exception: System.Exception: Tiny: C:\snapshot\manager\node_modules\babel-core\lib\transformation\file\options\option-manager.js:328
            throw e;
                    ^
            Error: Couldn't find preset "es2017" relative to directory "C:\\Users\\Ethan"


# Building development

1. If development build shows nothing.
1. Open web browser development console log.
1. If an error, then:
1. Edit index.html
1. Change invalid path containing with "/artifacts/" to relative path "../artifacts/"


# Highlighting TypeScript in Vim

1. Install <https://github.com/tpope/vim-pathogen/>
    1. Append to `~/.vimrc`:

            execute pathogen#infect()

1. Install TypeScript Vim syntax highlighting <https://github.com/leafgarland/typescript-vim>
