#!/usr/bin/env python3
import os, sys

FOLDERS = [
    # 'Inmobiliaria/Models',
    sys.argv[1]
]

# read all .cs in folder
def read_files(folder):
    files = []
    for root, dirs, filenames in os.walk(folder):
        for filename in filenames:
            if filename.endswith('.cs') or filename.endswith('.cshtml'):
                files.append(os.path.join(root, filename))
    return files

if __name__ == '__main__':
    for folder in FOLDERS:
        files = read_files(folder)
        for file in files:
            print('# ' + file)
            print('---')
            with open(file, 'r') as f:
                content = f.read()
                print(content)