import os
import json
from collections import Counter
import subprocess

# Aktuellen Branch ermitteln
branch = subprocess.check_output(['git', 'rev-parse', '--abbrev-ref', 'HEAD']).decode().strip()

filetypes = []
for root, dirs, files in os.walk('.'):
    if '.git' in root or '.github' in root:
        continue
    for f in files:
        if '.' in f:
            ext = f[f.rfind('.'):]
            filetypes.append(ext.lower())
        else:
            filetypes.append('(no ext)')

counter = Counter(filetypes)
result = {
    "branch": branch,
    "filetypes": dict(counter)
}

# Ergebnis als JSON-Zeile anhängen
with open("results.txt", "a", encoding="utf-8") as f:
    f.write(json.dumps(result) + "\n")
