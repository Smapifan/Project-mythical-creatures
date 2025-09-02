import os
import json
from collections import Counter
import subprocess

# Funktion um alle Branches zu bekommen
branches = subprocess.check_output(['git', 'branch', '-r']).decode().splitlines()
branches = [b.strip().replace('origin/', '') for b in branches if 'HEAD' not in b]

# Ergebnisse sammeln
all_results = []

for branch in branches:
    # Temporär zum Branch wechseln
    subprocess.run(['git', 'checkout', branch], stdout=subprocess.DEVNULL, stderr=subprocess.DEVNULL)
    
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
    
    if not filetypes:
        filetypes.append('(no files found)')
    
    counter = Counter(filetypes)
    result = {
        "branch": branch,
        "filetypes": dict(counter)
    }
    all_results.append(result)

# Ergebnisse immer in results.txt schreiben
results_path = os.path.join('.github', 'results.txt')
with open(results_path, "w", encoding="utf-8") as f:
    for r in all_results:
        f.write(json.dumps(r) + "\n")
