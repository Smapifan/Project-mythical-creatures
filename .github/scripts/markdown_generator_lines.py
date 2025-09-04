import os
import json
from pathlib import Path

input_dir = "All_Code"
output_file = "FILECODE.md"

rows = []

for file in Path(input_dir).glob("*.json"):
    with open(file, "r", encoding="utf-8") as f:
        data = json.load(f)
    branch = data["branch"]
    total = data["total_lines"]
    by_ext = data["lines_by_ext"]

    rows.append(f"### Branch `{branch}`\n")
    rows.append("| Sprache | Zeilen | Prozent |\n")
    rows.append("|----------|--------|---------|\n")
    for ext, count in sorted(by_ext.items(), key=lambda x: -x[1]):
        perc = (count / total) * 100 if total else 0
        rows.append(f"| `{ext}` | {count} | {perc:.2f}% |\n")
    rows.append(f"| **Total** | **{total}** | 100% |\n\n")

with open(output_file, "w", encoding="utf-8") as f:
    f.write("## 📊 Code Line Counts (all Branches)\n\n")
    f.writelines(rows)

print(f"Markdown erstellt: {output_file}")
