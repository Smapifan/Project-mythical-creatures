import os
import json
import subprocess
from collections import Counter

# Liste der Branches, die wir berücksichtigen wollen
branches_to_scan = ["main", "Wiki"]

output_dir = "Results"
os.makedirs(output_dir, exist_ok=True)

for branch in branches_to_scan:
    # Zum Branch wechseln
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
        "filetypes": dict(counter),
        "total_files": sum(counter.values())
    }

    json_path = os.path.join(output_dir, f"Result{branch}.json")
    with open(json_path, "w", encoding="utf-8") as jf:
        json.dump(result, jf, indent=2)
