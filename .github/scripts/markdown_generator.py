import os
import json

output_dir = "Results"
lines = ["## 🗂️ File Types (alle Branches)\n"]

for filename in sorted(os.listdir(output_dir)):
    if not filename.endswith(".json"):
        continue
    path = os.path.join(output_dir, filename)
    with open(path, encoding="utf-8") as f:
        data = json.load(f)

    branch = data["branch"]
    filetypes = data["filetypes"]
    total = data["total_files"]

    lines.append(f"### Branch `{branch}`")
    lines.append("| File Type | Count | Percentage |")
    lines.append("|-----------|-------|------------|")

    # Sortieren: andere Dateitypen nach Count absteigend, 'other' immer ans Ende
    sorted_items = sorted(
        ((ext, cnt) for ext, cnt in filetypes.items() if ext != 'other'),
        key=lambda x: -x[1]
    )

    if 'other' in filetypes:
        sorted_items.append(('other', filetypes['other']))

    for ext, count in sorted_items:
        percent = f"{(count/total)*100:.2f}%"
        lines.append(f"| `{ext}` | {count} | {percent} |")

    lines.append("")

with open("FILETYPES.md", "w", encoding="utf-8") as f:
    f.write("\n".join(lines))

print("Markdown FILETYPES.md erfolgreich erstellt.")
